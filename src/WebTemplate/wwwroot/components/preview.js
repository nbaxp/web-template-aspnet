import { ArrowDown, ArrowUp, DocumentCopy } from '@element-plus/icons-vue';
import { useClipboard } from '@vueuse/core';
import hljs from 'highlight.js';
import html from 'template';
import { defineAsyncComponent, onMounted, reactive } from 'vue';

const template = html` <el-card>
    <template #header>
      <div style="display:flex;justify-content: space-between;align-items: center;">
        <span>{{title}}</span>
        <div>
          <el-button circle @click="copy(model.source)">
            <el-icon><document-copy /></el-icon>
          </el-button>
          <el-button circle @click="toggleSource">
            <el-icon v-if="model.showSource"><arrow-down /></el-icon>
            <el-icon v-else><arrow-up /></el-icon>
          </el-button>
        </div>
      </div>
    </template>
    <template v-if="model.source && model.component">
      <component :is="model.component" v-bind="props" />
    </template>
  </el-card>
  <el-card v-show="model.showSource">
    <pre><code class="hljs language-javascript" v-html="model.formatSource"></code></pre>
  </el-card>`;

export default {
  props: {
    title: {
      default: '',
    },
    name: {
      default: '',
    },
  },
  template,
  components: { DocumentCopy, ArrowUp, ArrowDown },
  setup(props) {
    const { copy } = useClipboard();
    let model = reactive({
      source: null,
      formatSource: null,
      showSource: false,
      component: null,
      props: props,
    });
    onMounted(async () => {
      try {
        const url = `../assets/components/${props.name}.js`;
        const response = await fetch(url);
        const result = await response.text();
        model.source = result;
        model.formatSource = hljs.highlight(result, { language: 'javascript' }).value;
        const dataUri = 'data:text/javascript;charset=utf-8,' + encodeURIComponent(result + '\n//@ sourceURL=' + url);
        model.component = defineAsyncComponent(() => import(dataUri));
        console.log(model.component);
      } catch (error) {
        model.source = error;
      }
    });
    return {
      model,
      props,
      copy,
      toggleSource() {
        model.showSource = !model.showSource;
      },
    };
  },
};
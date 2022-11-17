// https://github.com/markedjs/marked/
// https://github.com/highlightjs/highlight.js/
// https://github.com/mermaid-js/mermaid
import hljs from 'highlight.js';
import { marked, setOptions } from 'marked';
import mermaid from 'mermaid';
import html from 'template';
import { nextTick, onMounted, ref } from 'vue';

const template = html` <div class="markdown-body" v-html="source" ref="rootRef"></div> <div id="test111"></div>`;
const styles = html` <style>
  .markdown-body {
    box-sizing: border-box;
    margin: 0 auto;
  }
</style>`;

export default {
  props: {
    name: {
      default: 'file',
    },
  },
  template,
  styles,
  setup(props) {
    mermaid.initialize({ startOnLoad: false });
    let id = 0;
    const rootRef = ref(null);
    const source = ref('');
    onMounted(async () => {
      try {
        setOptions({
          highlight: function (code, lang) {
            if (lang === 'mermaid') {
              return mermaid.mermaidAPI.render(`mermaid${id++}`, code, undefined);
            } else {
              const language = hljs.getLanguage(lang) ? lang : 'plaintext';
              return hljs.highlight(code, { language }).value;
            }
          },
          langPrefix: 'hljs language-',
        });
        const response = await fetch(`./assets/markdown/${props.name}.md`);
        const result = await response.text();
        source.value = marked(result);
        nextTick(() => {
          console.log(rootRef);
        });
      } catch (error) {
        source.value = error;
      }
    });
    return {
      source,
    };
  },
};
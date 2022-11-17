import html from 'template';
import { ref } from 'vue';

export default {
  template: html`<el-button type="primary" style="margin-left: 16px" @click="drawer = true"> open </el-button>
    <el-drawer v-model="drawer" title="I am the title" :with-header="false">
      <span>Hi there!</span>
    </el-drawer>`,
  setup() {
    const drawer = ref(false);
    return {
      drawer,
    };
  },
};
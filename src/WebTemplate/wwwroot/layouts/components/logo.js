import html from 'template';

import { useAppStore } from '../../store/index.js';

export default {
  template: html`<router-link to="/" class="logo-link">
    <el-space>
      <img alt="logo" src="./assets/images/logo.svg" />
      <h1 v-if="!appStore.menuCollapse">Name</h1>
    </el-space>
  </router-link> `,
  setup() {
    const appStore = useAppStore();
    return {
      appStore,
    };
  },
};
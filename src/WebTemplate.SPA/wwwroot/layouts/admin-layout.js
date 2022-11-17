import { useAppStore } from 'store';
import html from 'template';

import LayoutBreadcrumb from './components/breadcrumb.js';
import LayoutFooter from './components/footer.js';
import LayoutHeader from './components/header.js';
import LayoutMenu from './components/menu.js';

const template = html`
  <el-container class="root">
    <el-header><layout-header hasAside /></el-header>
    <el-container class="body">
      <el-aside :width="appStore.menuCollapse?'auto':appStore.asideWidth">
        <layout-menu />
      </el-aside>
      <el-container style="height:100%;overflow:auto;">
        <el-main>
          <layout-breadcrumb />
          <router-view />
        </el-main>
        <el-footer><layout-footer /></el-footer>
      </el-container>
    </el-container>
  </el-container>
`;

export default {
  components: { LayoutHeader, LayoutFooter, LayoutMenu, LayoutBreadcrumb },
  template,
  setup() {
    const appStore = useAppStore();
    return { appStore };
  },
};

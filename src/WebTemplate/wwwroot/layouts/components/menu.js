import { Expand, Fold, Location } from '@element-plus/icons-vue';
import { useAppStore } from 'store';
import html from 'template';
import { computed } from 'vue';

import SvgIcon from '../../components/svg-icon.js';
import LayoutLogo from './logo.js';
import MenuItem from './menu-item.js';

export default {
  template: html`<el-scrollbar>
    <el-menu :collapse="appStore.menuCollapse" :default-active="$route.fullPath" router>
      <el-menu-item :index="appStore.menu.current">
        <el-icon><svg-icon v-if="current.icon" :name="current.icon" /></el-icon>
        <template #title>
          <span>{{current?.title}}</span>
        </template>
      </el-menu-item>
      <template v-if="current.children">
        <menu-item v-for="item in current.children" v-model="item" />
      </template>
    </el-menu>
  </el-scrollbar> `,
  components: { LayoutLogo, Expand, Fold, Location, MenuItem, SvgIcon },
  setup() {
    const appStore = useAppStore();
    const current = computed(() => appStore.menu.items.find((o) => o.path === appStore.menu.current));
    return { appStore, current };
  },
};
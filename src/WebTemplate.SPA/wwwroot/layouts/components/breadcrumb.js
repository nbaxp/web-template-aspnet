import { useAppStore } from 'store';
import html from 'template';
import { computed } from 'vue';
import { useRoute } from 'vue-router';

import SvgIcon from '../../components/svg-icon.js';
import menuItem from './menu-item.js';

const template = html`<el-space direction="vertical">
  <el-breadcrumb separator="/" v-if="appStore.showBreadcrumb">
    <el-breadcrumb-item :to="{ path: '/' }">
      <el-space :size="5">
        <el-icon><svg-icon name="home" /></el-icon>
      </el-space>
    </el-breadcrumb-item>
    <template v-for="item in paths">
      <el-breadcrumb-item :to="{path:item.path}">
        <el-space :size="5">
          <el-icon v-if="item.icon"><svg-icon :name="item.icon" /></el-icon>
          <span>{{item.title}}</span>
        </el-space>
      </el-breadcrumb-item>
    </template>
  </el-breadcrumb>
</el-space>`;

export default {
  template,
  components: { SvgIcon },
  setup() {
    const appStore = useAppStore();
    const route = useRoute();
    const find = (list, key, value, paths) => {
      let result = null;
      for (const item of list) {
        if (item[key] === value) {
          paths.push(item);
          result = item;
          break;
        } else if (item.children) {
          paths.push(item);
          result = find(item.children, key, value, paths);
        }
      }
      return result;
    };

    const paths = computed(() => {
      const paths = [];
      find(appStore.menu.items, 'path', route.path, paths);
      console.log(menuItem, paths);
      return paths;
    });
    return {
      appStore,
      paths,
    };
  },
};

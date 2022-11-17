import { useAppStore } from 'store';
import html from 'template';
import { computed, watchEffect } from 'vue';

import en from './libs/element-plus/locale/en.min.mjs';
import zhCn from './libs/element-plus/locale/zh-cn.min.mjs';

const template = html`
  <el-config-provider :size="appStore.size" :locale="locale">
    <router-view />
  </el-config-provider>
`;

export default {
  components: { zhCn, en },
  template,
  setup() {
    const appStore = useAppStore();
    watchEffect(() => {
      appStore.toggleTheme(appStore.theme === 'dark');
    });
    watchEffect(() => {
      if (appStore.theme === 'auto') {
        appStore.toggleTheme(appStore.isDark);
      }
    });
    const locales = new Map([
      ['zh-CN', zhCn],
      ['en-US', en],
    ]);
    const locale = computed(() => {
      return locales.get(appStore.locale);
    });
    return {
      appStore,
      locale,
    };
  },
};

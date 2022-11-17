import { useMediaQuery } from '@vueuse/core';
import { defineStore } from 'pinia';

import settings from '../config/settings.js';

export default defineStore(import.meta.url, {
  state: () => {
    Object.assign(settings, localStorage.getItem('settings'));
    const isDark = useMediaQuery('(prefers-color-scheme: dark)');
    return {
      ...settings,
      isDark,
      showSettingPanel: false,
      menu: {
        current: '/',
        items: [
          {
            title: '首页',
            path: '/',
          },
          {
            title: 'Element Plus 组件',
            path: '/el',
          },
          {
            title: '管理中心',
            path: '/admin',
            icon: 'file',
            // hide: true,
            children: [
              {
                title: '图表',
                path: '/admin/chart',
                icon: 'file',
              },
              {
                title: '表格',
                path: '/admin/list',
                icon: 'file',
              },
            ],
          },
        ],
      },
    };
  },
  actions: {
    toggleSettingPanel() {
      this.showSettingPanel = !this.showSettingPanel;
    },
    toggleTheme: (isDark) => {
      const darkClass = 'dark';
      if (isDark) {
        document.body.classList.add(darkClass);
      } else {
        document.body.classList.remove(darkClass);
      }
    },
    toggleMenu() {
      this.menuCollapse = !this.menuCollapse;
    },
    api(url) {
      return `${this.apiPrefix}${url}`;
    },
  },
});

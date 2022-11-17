import { useMediaQuery } from '@vueuse/core';
import { defineStore } from 'pinia';

import settings from '../config/settings.js';

export default defineStore(import.meta.url, {
  state: () => {
    return {
      ...settings,
      isDark: useMediaQuery('(prefers-color-scheme: dark)'),
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
    async init() {
      // Object.assign(settings, localStorage.getItem('settings'));
      try {
        const url = await this.api('/locale/list');
        const options = { headers: { 'Content-Type': 'application/json' }, };
        const response = await fetch(url, options);
        const result = await response.json();
        this.locale.current = result.current;
        this.locale.options = result.options;

        const url2 = await this.api('/locale/resources');
        const response2 = await fetch(url2, options);
        const result2 = await response2.json();
        this.locale.resources = result2;
      } catch (e) {
        console.error(e);
      }
    },
    toggleSettingPanel() {
      this.showSettingPanel = !this.showSettingPanel;
    },
    toggleTheme: (isDark) => {
      const darkClass = 'dark';
      if (isDark) {
        document.documentElement.classList.add(darkClass);
      } else {
        document.documentElement.classList.remove(darkClass);
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
import { useAppStore } from 'store';
import { createI18n } from 'vue-i18n';

import en from './en-US/index.js';
import cn from './zh-CN/index.js';

export const LOCALE_OPTIONS = [
  { value: 'en-US', text: 'English' },
  { value: 'zh-CN', text: '中文' },
];

export default function () {
  const appStore = useAppStore();
  const i18n = createI18n({
    locale: appStore.locale,
    fallbackLocale: 'zh-CN',
    allowComposition: true,
    messages: {
      'en-US': en,
      'zh-CN': cn,
    },
  });
  return i18n;
}

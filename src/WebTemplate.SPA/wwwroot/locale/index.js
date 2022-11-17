import { useAppStore } from 'store';
import { createI18n } from 'vue-i18n';

import en from './en-US/index.js';
import cn from './zh-CN/index.js';

export default function () {
  const appStore = useAppStore();
  const config = {
    locale: appStore.locale.current,
    fallbackLocale: 'zh-Hans-CN',
    allowComposition: true,
    messages: {},
  };
  config.messages[appStore.locale.current] = appStore.locale.resources;
  const i18n = createI18n(config);
  return i18n;
}
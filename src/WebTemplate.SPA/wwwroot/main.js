import { createApp } from 'vue';
import ElementPlus from 'element-plus';
// import useMock from './mock/index.js';
// useMock();
import router from './router/index.js';
import store from 'store';
import i18n from './locale/index.js';

import { useAppStore, useUserStore } from 'store';

import App from './app.js';
import style from './mixins/style.js';

const app = createApp(App);

app.use(store);

await useAppStore().init();
await useUserStore().init();

app.use(ElementPlus);

app.use(router);
app.use(i18n());


app.mixin(style);

app.mount('#app');

import ElementPlus from 'element-plus';
// import useMock from './mock/index.js';
// useMock();
import store from 'store';
import { useUserStore } from 'store';
import { createApp } from 'vue';

import App from './app.js';
import locale from './locale/index.js';
import style from './mixins/style.js';
import router from './router/index.js';

const app = createApp(App);
app.use(store);
app.use(locale());
const userStore = useUserStore();
await userStore.init();
app.use(router);
app.use(ElementPlus);
app.mixin(style);
app.mount('#app');

import { createPinia } from 'pinia';

import useAppStore from './app.js';
import useUserStore from './user.js';

const pinia = createPinia();

export { useAppStore, useUserStore };
export default pinia;

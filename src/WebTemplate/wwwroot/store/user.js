import jwt_decode from 'jwt-decode';
import { defineStore } from 'pinia';
import { useAppStore } from 'store';

import router from '../router/index.js';
import { post } from '../utils/request.js';

const LOGOUT_KEY = 'logout';

export default defineStore(import.meta.url, {
  states: () => {
    return {
      timer: null,
      token: null,
      user: null,
    };
  },
  actions: {
    async login(url, data) {
      const response = await post(url, data);
      if (response.status === 200) {
        const result = await response.json();
        this.setToken(result);
        this.start();
        const route = router.currentRoute.value;
        router.push(route.query.redirect ?? '/');
      }
    },
    async logout() {
      const appStore = useAppStore();
      const url = appStore.api('/logout');
      await post(url);
      localStorage.removeItem(LOGOUT_KEY);
      this.clear();
    },
    async setToken(data) {
      this.token = data.access_token;
      if (!localStorage.getItem(LOGOUT_KEY)) {
        localStorage.setItem(LOGOUT_KEY, Date.now().toLocaleString());
      }
    },
    clear() {
      if (this.timer) {
        clearInterval(this.timer);
        this.timer = null;
      }
      this.token = null;
      const route = router.currentRoute.value;
      if (route.meta?.requiresAuth) {
        router.push({ path: '/login', query: { redirect: route.fullPath } });
      }
    },
    async refresh() {
      const appStore = useAppStore();
      const url = appStore.api('/token/refresh');
      try {
        const response = await post(url);
        if (response.status === 200) {
          const result = await response.json();
          this.setToken(result);
          return true;
        } else {
          console.error(response.statusText);
        }
      } catch (error) {
        console.log(error);
      }
      return false;
      // router.push('/login');
    },
    async start() {
      if (!this.timer) {
        this.timer = setInterval(async () => {
          const exp = jwt_decode(this.token).exp * 1000;
          if (exp - Date.now() < 1000 * 15) {
            console.debug(`????????????token,???????????????${new Date(exp).toLocaleString()}`);
            if (await this.refresh()) {
              console.log('????????????');
            } else {
              console.log('????????????');
              await this.logout();
            }
          } else {
            console.debug(`????????????token,???????????????${new Date(exp).toLocaleString()}`);
          }
        }, 1000 * 10);
      }
    },
    async init() {
      window.addEventListener(
        'storage',
        (event) => {
          console.log(event);
          if (event.key === LOGOUT_KEY && !event.newValue) {
            this.clear();
          }
        },
        { once: true }
      );
      if (!this.token) {
        if (await this.refresh()) {
          await this.start();
        }
      }
    },
  },
});

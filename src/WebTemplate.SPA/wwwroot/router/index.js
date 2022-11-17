import NProgress from 'nprogress';
import { useUserStore } from 'store';
import { useAppStore } from 'store';
import { createRouter, createWebHistory } from 'vue-router';

import AdminLayout from '../layouts/admin-layout.js';
import DefaultLayout from '../layouts/layout.js';
import AdminChart from '../views/admin/chart.js';
import AdminIndex from '../views/admin/index.js';
import AdminList from '../views/admin/list.js';
import EL from '../views/el.js';
import GenericEntity from '../views/generic-entity.js';
import Home from '../views/home.js';
import Login from '../views/login.js';
import NotFound from '../views/not-found.js';
import Test from '../views/test.js';

NProgress.configure({ showSpinner: false });

const routes = [
  {
    path: '/',
    component: DefaultLayout,
    children: [
      {
        path: '/',
        component: Home,
      },
      {
        path: 'el',
        component: EL,
      },
      {
        path: 'test',
        component: Test,
      },
    ],
  },
  {
    path: '/admin',
    component: AdminLayout,
    children: [
      {
        path: '',
        component: AdminIndex,
        meta: {
          requiresAuth: true,
        },
      },
      {
        path: 'chart',
        component: AdminChart,
        meta: {
          requiresAuth: true,
        },
      },
      {
        path: 'list',
        component: AdminList,
        meta: {
          requiresAuth: true,
        },
      },
    ],
  },
  {
    path: '/login',
    component: Login,
  },
  { path: '/entity/:entity(.*)*', component: GenericEntity },
  { path: '/:pathMatch(.*)*', component: NotFound },
];

const router = new createRouter({
  history: createWebHistory(),
  routes: routes,
});

router.beforeEach(async (to, from, next) => {
  NProgress.start();
  try {
    console.log(`router.beforeEach: ${from.path}->${to.path}`);
    const userStore = useUserStore();
    if (to.path !== '/login' && to.meta?.requiresAuth) {
      if (!userStore.token) {
        next({ path: '/login', query: { redirect: to.fullPath } });
      } else {
        next();
      }
    } else {
      next();
    }
  } catch (error) {
    console.log(error);
  } finally {
    NProgress.done();
  }
});

router.afterEach((to, from) => {
  console.log(`router.afterEach: ${from.path}->${to.path}`);
  NProgress.done();
  const appStore = useAppStore();
  appStore.menu.current = to.path.lastIndexOf('/') > 0 ? appStore.menu.items.find((o) => o.path !== '/' && to.path.startsWith(o.path))?.path : to.path;
  if (to.meta.title) {
    document.title = `${to.meta.title}`;
  }
});

export default router;

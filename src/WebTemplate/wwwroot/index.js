import { createApp, ref, reactive, onMounted } from 'vue';
import { defineStore } from 'pinia';
import ElementPlus from 'element-plus';
import en from './libs/element-plus/locale/en.min.mjs';
import zhCn from './libs/element-plus/locale/zh-cn.min.mjs';
import { ElMessage, ElMessageBox } from 'element-plus';
import { ArrowDown, Expand, Fold, Message, Setting, SwitchButton, User } from '@element-plus/icons-vue';
import { post } from '/utils/request.js';

// useApp
function useApp(ViewModel, logoutUrl, localeUrl, cultureItems) {
  const App = {
    components: { ViewModel, ArrowDown },
    template: '#appComponent',
    setup() {
      console.log('app init');
      const loading = ref(true);
      onMounted(() => {
        loading.value = false;
      });
      const menus = reactive([]);
      const menuCollapse = ref(false);
      const toggleMenu = () => menuCollapse.value = !menuCollapse.value;
      const hasAside = ref(false);
      const localization = reactive({
        items: cultureItems,
        getText() {
          return this.items.find(o => o.selected).text;
        },
        change(value) {
          var params = new URLSearchParams();
          params.set('language', value);
          params.set('target', location.href);
          location.href = `${localeUrl}?${params}`;
        }
      });
      const elLocale = localization.items.find(o => o.selected).value === 'zh' ? zhCn : en;
      const confirmLogout = async () => {
        try {
          await ElMessageBox.confirm('确认退出？', '提示', { type: 'warning' });
          try {
            var logoutUrl = '@Url.Action("Logout","Account")';
            const response = await post(logoutUrl);
            if (response.status === 200) {
              location.reload();
            }
          } catch (e) {
            console.error(e);
          }
          ElMessage({
            type: 'success',
            message: '退出成功',
          });
        } catch (error) {
          console.log(error);
          ElMessage({
            type: 'info',
            message: '退出取消',
          });
        }
      };
      return {
        loading,
        menus,
        menuCollapse,
        hasAside,
        localization,
        elLocale,
        toggleMenu,
        confirmLogout
      };
    }
  };
  const app = createApp(App);
  app.use(ElementPlus);
  return app;
}

export { useApp };
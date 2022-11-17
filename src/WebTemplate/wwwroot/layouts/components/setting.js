import { Moon, Platform, Sunny } from '@element-plus/icons-vue';
import html from 'template';
import { reactive, ref, watchEffect } from 'vue';

import { LOCALE_OPTIONS } from '../../locale/index.js';
import { useAppStore } from '../../store/index.js';

export default {
  template: html`<el-drawer title="页面配置" v-model="appStore.showSettingPanel">
    <el-form ref="formRef" :model="appStore" label-width="auto">
      <el-form-item :prop="appStore.theme" label="主题模式">
        <el-select v-model="appStore.theme">
          <template #prefix>
            <el-icon><component :is="themeOptions.find(o=>o.value===appStore.theme).icon" /></el-icon>
          </template>
          <template v-for="item in themeOptions">
            <el-option :key="item.value" :value="item.value" :label="item.text">
              <el-space>
                <el-icon><component :is="item.icon" /></el-icon>
                <span>{{item.text}}</span>
              </el-space>
            </el-option>
          </template>
        </el-select>
      </el-form-item>
      <el-form-item :prop="appStore.themeColor" label="主题色">
        <el-color-picker v-model="appStore.themeColor" />
      </el-form-item>
      <el-form-item :prop="appStore.locale" label="默认语言">
        <el-select v-model="appStore.locale">
          <template v-for="item in LOCALE_OPTIONS">
            <el-option :key="item.value" :value="item.value" :label="item.text"> <span>{{item.text}}</span></el-option>
          </template>
        </el-select>
      </el-form-item>
      <el-form-item :prop="appStore.size" label="组件大小">
        <el-select v-model="appStore.size">
          <template v-for="item in sizeOptions">
            <el-option :key="item.value" :value="item.value" :label="item.text"> <span>{{item.text}}</span></el-option>
          </template>
        </el-select>
      </el-form-item>
      <el-form-item :prop="appStore.showBreadcrumb" label="站内导航">
        <el-switch v-model="appStore.showBreadcrumb" />
      </el-form-item>
      <el-form-item>
        <el-button @click="resetForm">恢复默认</el-button>
      </el-form-item>
    </el-form>
  </el-drawer>`,
  components: { Sunny, Moon, Platform },
  setup() {
    const formRef = ref(null);
    const appStore = useAppStore();
    const themeOptions = reactive([
      { text: '跟随系统', value: 'auto', icon: Platform },
      { text: '亮色模式', value: 'light', icon: Sunny },
      { text: '暗色模式', value: 'dark', icon: Moon },
    ]);
    const sizeOptions = reactive([
      { text: '默认', value: 'default' },
      { text: '小', value: 'small' },
      { text: '大', value: 'large' },
    ]);
    watchEffect(() => {
      document.documentElement.style.setProperty('--el-color-primary', appStore.themeColor);
    });
    const resetForm = () => {
      formRef.value.resetFields();
      appStore.$reset();
    };
    return {
      appStore,
      themeOptions,
      sizeOptions,
      LOCALE_OPTIONS,
      formRef,
      resetForm,
    };
  },
};
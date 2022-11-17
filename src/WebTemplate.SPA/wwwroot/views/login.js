import { Lock, User } from '@element-plus/icons-vue';
import { useAppStore, useUserStore } from 'store';
import html from 'template';
import { reactive, ref } from 'vue';

import LayoutFooter from '../layouts/components/footer.js';
import LayoutLogo from '../layouts/components/logo.js';
import { getRules } from '../utils/form.js';

export default {
  template: html`<div class="width100 height100 flex-center">
    <div>
      <div class="flex-center padding"><layout-logo /></div>
      <el-card class="container">
        <el-form ref="formRef" size="large" :model="formModel" style="width:300px;" :roles="formRoles">
          <el-form-item :prop="formModel.username">
            <el-input
              v-model="formModel.username"
              :prefix-icon="formModelSchema.properties.username.icon"
              :placeholder="formModelSchema.properties.username.title"
            />
          </el-form-item>
          <el-form-item :prop="formModel.password">
            <el-input
              v-model="formModel.password"
              :prefix-icon="formModelSchema.properties.password.icon"
              :placeholder="formModelSchema.properties.password.title"
              :type="formModelSchema.properties.password.inputType"
            />
          </el-form-item>
          <el-form-item :prop="formModel.password" class="justify-content-space-between">
            <el-checkbox v-model="formModel.rememberMe">{{formModelSchema.properties.rememberMe.title}}</el-checkbox>
            <router-link class="router-link" to="/">忘记密码?</router-link>
          </el-form-item>
          <el-button type="primary" class="width100" @click="submitForm(formRef)">登录</el-button>
          <router-link to="/" class="router-link flex-center padding">注册</router-link>
        </el-form>
      </el-card>
      <div class="flex-center padding"> <layout-footer /></div>
    </div>
  </div>`,
  components: { LayoutLogo, LayoutFooter, User, Lock },
  setup() {
    const formRef = ref(null);
    const formModel = reactive({
      username: null,
      password: null,
      rememberMe: false,
    });
    const formModelSchema = reactive({
      properties: {
        username: {
          title: '用户名',
          icon: User,
          rules: [
            {
              required: true,
              trigger: 'change',
            },
          ],
        },
        password: {
          title: '密码',
          inputType: 'password',
          icon: Lock,
          rules: [
            {
              required: true,
              trigger: 'change',
            },
          ],
        },
        rememberMe: {
          title: '自动登录',
          inputType: 'checkbox',
        },
      },
    });
    const formRoles = reactive(getRules(formModelSchema));
    const appStore = useAppStore();
    const formAction = appStore.api('/token');
    const userStore = useUserStore();
    const submitForm = async (form) => {
      try {
        await form.validate();
        await userStore.login(formAction, formModel);
      } catch (e) {
        console.error(e);
      }
    };
    return {
      formRef,
      formModel,
      formModelSchema,
      formRoles,
      submitForm,
    };
  },
};
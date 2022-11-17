import html from 'template';

import SvgIcon from '../../components/svg-icon.js';

const template = html`<template v-if="modelValue">
  <el-sub-menu :index="modelValue.path" v-if="modelValue.children">
    <el-icon><svg-icon v-if="modelValue.icon" :name="modelValue.icon" /></el-icon>
    <template #title>
      <span>{{modelValue.title}}</span>
    </template>
    <menu-item v-for="item in modelValue.children" v-model="item" />
  </el-sub-menu>
  <el-menu-item :index="modelValue.path" v-else>
    <el-icon><svg-icon v-if="modelValue.icon" :name="modelValue.icon" /></el-icon>
    <template #title>
      <span>{{modelValue.title}}</span>
    </template>
  </el-menu-item>
</template>`;
export default {
  template,
  components: { SvgIcon },
  props: {
    modelValue: {
      type: Object,
      default: null,
    },
  },
};

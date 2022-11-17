import { Document, Location, Menu as IconMenu, Setting } from '@element-plus/icons-vue';
import html from 'template';

import MarkDown from '../components/mark-down.js';

const template = html`
  <mark-down name="home" />
  <el-calendar />
  <el-link href="https://staging-cn.vuejs.org/">vue</el-link>
  <br />
  <el-link href="https://next.router.vuejs.org/">vue router</el-link>
  <br />
  <el-link href="https://pinia.vuejs.org/">pinia</el-link>
  <br />
  <el-link href="https://vueuse.org/">vue use</el-link>
  <br />
  <el-link href="https://element-plus.gitee.io/zh-CN.html">element plus</el-link>
  <br />
  <el-link href="https://arco.design/vue/docs/start">arco design</el-link>
  <br />
  <el-link href="http://vue-pro.arco.design/dashboard/workplace">arco design pro</el-link>
  <br />
  <el-link href="https://tdesign.tencent.com/vue-next/overview">tdesign</el-link>
  <br />
  <el-link href="https://tdesign.tencent.com/starter/vue-next/">tdesign stater</el-link>
  <br />
  <el-link href="https://preview.pro.ant.design/dashboard/analysis/">ant design pro</el-link>
`;
export default {
  components: { Document, IconMenu, Location, Setting, MarkDown },
  template,
  setup() {
    const handleOpen = (key, keyPath) => {
      console.log(key, keyPath);
    };
    const handleClose = (key, keyPath) => {
      console.log(key, keyPath);
    };
    return { handleOpen, handleClose };
  },
};

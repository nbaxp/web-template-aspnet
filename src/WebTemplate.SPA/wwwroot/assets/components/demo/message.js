import { ElMessage } from 'element-plus';
import html from 'template';
import { h } from 'vue';

export default {
  template: html`<el-button :plain="true" @click="open">Show message</el-button> <el-button :plain="true" @click="openVn">VNode</el-button>`,
  setup() {
    const open = () => {
      ElMessage({ showClose: true, duration: 0, message: 'this is a message.' });
    };

    const openVn = () => {
      ElMessage({
        message: h('p', null, [h('span', null, 'Message can be '), h('i', { style: 'color: teal' }, 'VNode')]),
      });
    };
    return {
      open,
      openVn,
    };
  },
};

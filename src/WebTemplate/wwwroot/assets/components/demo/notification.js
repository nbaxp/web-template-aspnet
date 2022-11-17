import { ElNotification } from 'element-plus';
import html from 'template';
import { h } from 'vue';

export default {
  template: html`<el-button plain @click="open1"> Closes automatically </el-button> <el-button plain @click="open2"> Won't close automatically </el-button>`,
  setup() {
    const open1 = () => {
      ElNotification({
        title: 'Title',
        message: h('i', { style: 'color: teal' }, 'This is a reminder'),
      });
    };

    const open2 = () => {
      ElNotification({
        title: 'Prompt',
        message: 'This is a message that does not automatically close',
        duration: 0,
      });
    };
    return {
      open1,
      open2,
    };
  },
};
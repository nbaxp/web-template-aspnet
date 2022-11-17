import { ElMessageBox } from 'element-plus';
import html from 'template';
import { ref } from 'vue';

export default {
  template: html`<el-button type="text" @click="dialogVisible = true">click to open the Dialog</el-button>
    <el-dialog v-model="dialogVisible" title="Tips" width="30%" :before-close="handleClose">
      <span>This is a message</span>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="dialogVisible = false">Cancel</el-button>
          <el-button type="primary" @click="dialogVisible = false">Confirm</el-button>
        </span>
      </template>
    </el-dialog>`,
  styles: html`.dialog-footer button:first-child { margin-right: 10px; }`,
  setup() {
    const dialogVisible = ref(false);

    const handleClose = () => {
      ElMessageBox.confirm('Are you sure to close this dialog?')
        .then(() => {})
        .catch(() => {
          // catch error
        });
    };
    return {
      dialogVisible,
      handleClose,
    };
  },
};

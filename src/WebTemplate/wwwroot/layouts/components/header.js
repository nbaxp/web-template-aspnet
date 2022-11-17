import { ArrowDown, Expand, Fold, Message, Setting, SwitchButton, User } from '@element-plus/icons-vue';
import { ElMessage, ElMessageBox } from 'element-plus';
import { useAppStore, useUserStore } from 'store';
import html from 'template';

import SvgIcon from '../../components/svg-icon.js';
import LayoutLogo from './logo.js';
import LayoutSetting from './setting.js';

const template = html`
  <el-space>
    <layout-logo />
    <el-button v-if="hasAside" @click="appStore.toggleMenu">
      <el-icon> <fold v-if="appStore.menuCollapse" /><expand v-else /> </el-icon>
    </el-button>
    <el-menu mode="horizontal" :default-active="appStore.menu.current" :ellipsis="false" router>
      <template v-for="item in appStore.menu.items">
        <el-menu-item v-if="!item.hide" :index="item.path">
          <template #title>
            <el-icon v-if="item.icon"><svg-icon :name="item.icon" /></el-icon>
            <span>{{item.title}}</span>
          </template>
        </el-menu-item>
      </template>
    </el-menu>
  </el-space>
  <el-space>
    <template v-if="userStore.token">
      <router-link to="/" style="margin-right:2rm;">
        <el-badge :value="3" class="item">
          <el-button circle>
            <el-icon style="font:1.2em;"><Message /></el-icon>
          </el-button>
        </el-badge>
      </router-link>
      <el-dropdown>
        <el-space>
          <el-avatar size="small" src="./assets/images/avatar.svg" />
          <span>用户名</span>
          <el-icon><arrow-down /></el-icon>
        </el-space>
        <template #dropdown>
          <el-dropdown-menu>
            <el-dropdown-item>
              <el-icon><user /></el-icon>用户中心
            </el-dropdown-item>
            <el-dropdown-item>
              <el-icon><switch-button /></el-icon>设置
            </el-dropdown-item>
            <el-dropdown-item divided @click="confirmLogout">
              <el-icon><setting /></el-icon>退出登录
            </el-dropdown-item>
          </el-dropdown-menu>
        </template>
      </el-dropdown>
    </template>
    <template v-else>
      <router-link class="router-link" to="/login">登录</router-link>
      <router-link class="router-link" to="/register">注册</router-link>
    </template>
    <el-tooltip placement="bottom">
      <template #content>页面配置</template>
      <el-button circle @click="appStore.toggleSettingPanel">
        <el-icon class="cursor"><Setting /></el-icon>
      </el-button>
    </el-tooltip>
  </el-space>
  <layout-setting />
`;

export default {
  template,
  components: {
    Fold,
    Expand,
    ArrowDown,
    User,
    Setting,
    SwitchButton,
    LayoutSetting,
    LayoutLogo,
    Message,
    SvgIcon,
  },
  props: {
    hasAside: {
      type: Boolean,
      default: false,
    },
  },
  setup() {
    const appStore = useAppStore();
    const userStore = useUserStore();
    const confirmLogout = async () => {
      try {
        await ElMessageBox.confirm('确认退出？', '提示', { type: 'warning' });
        await userStore.logout();
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
      Setting,
      Message,
      appStore,
      userStore,
      confirmLogout,
    };
  },
};
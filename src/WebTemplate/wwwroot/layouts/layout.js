import html from 'template';

import LayoutFooter from './components/footer.js';
import LayoutHeader from './components/header.js';

const template = html`
  <el-container>
    <el-header><layout-header /></el-header>
    <el-container class="body">
      <el-main><router-view /></el-main>
      <el-footer><layout-footer /> </el-footer>
      <el-backtop target=".body" />
    </el-container>
  </el-container>
`;

export default {
  components: { LayoutHeader, LayoutFooter },
  template,
  setup() {
    return {};
  },
};
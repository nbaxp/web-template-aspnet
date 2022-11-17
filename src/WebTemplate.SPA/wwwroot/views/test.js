import html from 'template';

import SvgIcon from '../components/svg-icon.js';
import Test from '../components/test-comp.js';
import { Test2 } from '../components/test-comp.js';

const now = Date.now().toLocaleString();

const template = html`<h2>locale:{{ $t("test") }}</h2>
  <h2>now:${now}</h1>
  <Test />
  <Test />
  <Test2 />
  <SvgIcon name="folder" />`;

const styles = html` <style>
  h1 {
    color: red;
  }
</style>`;

export default {
  components: { Test, Test2, SvgIcon },
  template,
  styles,
};

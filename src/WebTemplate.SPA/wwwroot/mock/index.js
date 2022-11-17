import { useAppStore } from '../store/index.js';
import useUser from './user.js';

export default function () {
  const appStore = useAppStore();
  if (appStore.mock) {
    useUser();
  }
}

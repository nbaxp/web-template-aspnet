// https://stackoverflow.com/questions/9133500/how-to-find-a-node-in-a-tree-with-javascript
export default function (tree, value, key = 'id', reverse = false) {
  const stack = [tree[0]];
  while (stack.length) {
    const node = stack[reverse ? 'pop' : 'shift']();
    if (node[key] === value) return node;
    node.children && stack.push(...node.children);
  }
  return null;
}

export default async function (selector) {
  let source = document.querySelector(selector)?.innerHTML?.replaceAll(" from '/", ` from '${location.protocol}//${location.host}/`);
  const dataUri = "data:text/javascript;charset=utf-8," + encodeURIComponent(source);
  return (await import(dataUri)).default ?? {};
}
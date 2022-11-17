async function post(url, data) {
  let options = {
    method: 'post',
    credentials: 'include',
    headers: { 'Content-Type': 'application/json' },
  };
  if (data) {
    options.body = JSON.stringify(data);
  }
  return await fetch(url, options);
}

export { post };

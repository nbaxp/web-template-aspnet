async function post(url, data,requestToken) {
  let options = {
    method: 'post',
    credentials: 'include',
    headers: {
      'RequestVerificationToken': requestToken,
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
  };
  if (data) {
    options.body = JSON.stringify(data);
  }
  return await fetch(url, options);
}

export { post };

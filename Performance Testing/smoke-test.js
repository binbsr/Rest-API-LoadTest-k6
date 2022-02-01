import http from 'k6/http';
import { check, group, sleep, fail } from 'k6';

export const options = {
  vus: 1, // 1 user looping for 1 minute
  duration: '1m',

  thresholds: {
    http_req_duration: ['p(99)<1500'],
  },
};

const BASE_URL = 'https://localhost:7020';

export default () => {

  const myObjects = http.get(`${BASE_URL}/countries`).json();
  check(myObjects, { 'retrieved countries': (obj) => obj.length > 0 });

  sleep(1);
};

import http from 'k6/http';
import { check, group, sleep } from 'k6';

export const options = {
  stages: [
    { duration: '30s', target: 60 }, // simulate ramp-up of traffic from 1 to 60 users over 5 minutes.
    { duration: '1m', target: 60 }, // stay at 60 users for 10 minutes
    { duration: '30s', target: 100 }, // ramp-up to 100 users over 3 minutes (peak hour starts)
    { duration: '30s', target: 100 }, // stay at 100 users for short amount of time (peak hour)
    { duration: '30s', target: 60 }, // ramp-down to 60 users over 3 minutes (peak hour ends)
    { duration: '1m', target: 60 }, // continue at 60 for additional 10 minutes
    { duration: '30s', target: 0 }, // ramp-down to 0 users
  ],
  thresholds: {
    'http_req_duration': ['p(99)<1500']    
  },
};

const BASE_URL = 'https://localhost:7020';

export default () => {

  const myObjects = http.get(`${BASE_URL}/countries/`).json();
  check(myObjects, { 'retrieved countries': (obj) => obj.length > 0 });

  sleep(1);
};

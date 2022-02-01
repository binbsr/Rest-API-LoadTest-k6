import http from "k6/http"
import { sleep } from "k6"

export const options = {
    vus: 1,
    duration: "10s"
};

export default () => {
    http.get("https://localhost:7020/");
    sleep(1);
};

import http from 'k6/http';
import { sleep, check, fail } from 'k6';
import { Trend, Rate, Counter } from 'k6/metrics';

export let GetContactDuration = new Trend('get_contact_duration');
export let GetContactFailRate = new Trend('get_contact_fail_rate');
export let GetContactSuccessRate = new Trend('get_contact_success_rate');
export let GetContactReqs = new Trend('get_contact_reqs');

export default function GetPacientes(){
    let res = http.get('https://localhost:32001/api/Paciente');

    GetContactDuration.add(res.timings.duration);
    GetContactReqs.add(1);
    GetContactFailRate.add(res.status == 0 || res.status > 399);
    GetContactSuccessRate.add(res.status < 399);

    let durationMsg = `Max Duration ${4000/1000}s`;
    if (check(res, {
        'max duration': (r) => r.timings.duration < 4000,
    })){
        fail(durationMsg);
    }

    sleep(1);
}
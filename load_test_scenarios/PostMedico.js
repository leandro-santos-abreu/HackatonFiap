import http from 'k6/http';
import { sleep, check, fail } from 'k6';
import { Trend, Rate, Counter } from 'k6/metrics';

// Define custom metrics
export let PostContactDuration = new Trend('post_contact_duration');
export let PostContactFailRate = new Rate('post_contact_fail_rate');
export let PostContactSuccessRate = new Rate('post_contact_success_rate');
export let PostContactReqs = new Counter('post_contact_reqs');

export default function PostMedico() {

    let payload = JSON.stringify({
        Nome: 'John Doe',
        CRM: 'SP123456',
        Email: 'john.doe@example.com',
        Especialidade : 'Oftalmologista',
		Senha: "123456",
		CPF: "98805907081"
    });

    // Set headers (if necessary, like Content-Type for JSON)
    let headers = {
        'Content-Type': 'application/json',
    };

    // Send the POST request
    let res = http.post('https://localhost:32001/Medico', payload, { headers: headers });

    // Track metrics
    PostContactDuration.add(res.timings.duration); // Track the duration
    PostContactReqs.add(1); // Track the number of requests
    PostContactFailRate.add(res.status == 0 || res.status > 399); // Fail rate
    PostContactSuccessRate.add(res.status < 399); // Success rate

    // Check that the request duration is less than the threshold (e.g., 4000ms)
    let durationMsg = `Max Duration ${4000 / 1000}s`;
    if (check(res, {
        'max duration': (r) => r.timings.duration < 4000, // Ensure duration < 4 seconds
    })) {
        fail(durationMsg); // Fail the test if it exceeds the duration
    }

    // Add a delay between requests
    sleep(1); // Adjust sleep time as necessary
}

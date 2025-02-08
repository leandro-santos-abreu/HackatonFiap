import GetContacts from './GetContacts.js';
import PostContact from './PostContact.js';
import {group, sleep} from  'k6';

export default () => {
    group('Endpoint Get Contact - Controller Contact - PosTechFiap.Api', () =>{
        GetContacts();
    })

    // group('Endpoint Post Contact - Controller Contact - PosTechFiap.Api', () =>{
    //     PostContact();
    // })

    sleep(1);
}
import React, { useEffect, useState } from 'react';
import Cookies from 'universal-cookie';
import { encode as base64_encode } from 'base-64';
import envatoicon from '../images/envato_icon.png';
import logo from '../images/logo.png';

const cookies = new Cookies();

export default function Verify(props) {

    const [authenticationCode, setauthenticationCode] = useState("");
    const [authenticationResult, setauthenticationResult] = useState({});//{boolResult  ,description }
    const [activeSessions, setActiveSessions] = useState([]);
    const [isAuthenticate, setIsauthenticate] = useState(false);
    const [content, setContent] = useState(<></>);
    const [showActivaionCode, setShowActivaionCode] = useState(false);
    const [inputCode, setInputCode] = useState(''); // '' is the initial state value
    const [inputMail, setInputMail] = useState(''); // '' is the initial state value

    useEffect(() => {
        var _code = cookies.get('code');
        if (_code) {
            setauthenticationCode(_code);
            setIsauthenticate(true);
        } else {
            logOut();
        }

        cookies.removeChangeListener(() => {
            var _code = cookies.get('code');
            if (!_code) {
                logOut();
            }
        });
    }, []);

    const submitPurchaseCode = () => {
        var encodedData = base64_encode(inputCode.trim() + '|' + inputMail.trim() + '|' + authenticationCode + '|WonderCallOuts');
        fetch('https://wondertools-official.com/api/lic/envatocomplete/' + encodedData).then(response => {
        //fetch('http://localhost:9945/api/lic/envatocomplete/' + encodedData).then(response => {
            
            return response.json();
        }).then((data) => {
            setauthenticationResult(Object.assign({}, data)); //{boolResult  ,description }
            setShowActivaionCode(true);
            setActiveSessions(JSON.parse(data.stringResult));
        }).catch(err => {
            logOut();
            alert(err);
        })
    }


    const deActivateSeason = (id) => {
        var encodedData = base64_encode(id);
        fetch('https://wondertools-official.com/api/lic/deactive/' + encodedData).then(response => {
        //fetch('http://localhost:9945/api/lic/deactive/' + encodedData).then(response => {
            
            return response.json();
        }).then((data) => {
            if (data.boolResult == false) {
                alert(data.description);
                logOut();
            }
            setActiveSessions(JSON.parse(data.stringResult));
        }).catch(err => {
            logOut();
            alert(err);
        })
    }

    const logOut = () => {
        cookies.remove('code')
        setauthenticationCode("");
        setauthenticationResult("");
        setActiveSessions([]);
        setIsauthenticate(false);
        setShowActivaionCode(false);
        setInputCode("");
        setInputMail("");
    }

    const gotoEnvato = () => {
        //window.location.href = 'https://api.envato.com/authorization?response_type=code&client_id=wtactivation-aad8crdk&redirect_uri=http://localhost:9945/redirect';
        window.location.href = 'https://api.envato.com/authorization?response_type=code&client_id=wtactivation-aad8crdk&redirect_uri=https://wondertools-official.com/redirect';
    }

    useEffect(() => {
        var _content = <></>;
        if (!showActivaionCode) {
            if (isAuthenticate) {
                _content = (<>
                    <p className="mt-2 accent-color" >Successfully loged in!</p>
                    <div className="position-relative pb-1 text-left p-0">
                        <label htmlFor="txtCode" className="my-form-label">
                            Purchase Code *
                        </label>
                        <div className="input-group input-group-sm">
                            <input type='text'
                                autoComplete="false"
                                className="form-control text-light bg-dark rounded-pill border-light text-center"
                                id="txtCode" placeholder="xxxxxx-xxxxxx..."
                                value={inputCode} onInput={e => setInputCode(e.target.value)}
                                maxLength={80} />
                        </div>
                    </div>
                    <div className="position-relative pb-1 text-left p-0">
                        <label htmlFor="txtCode" className="my-form-label">
                            E-mail Address *
                        </label>
                        <div className="input-group input-group-sm">
                            <input type="email"
                                autoComplete="false"
                                className="form-control text-light bg-dark rounded-pill border-light text-center"
                                id="txtEmail" placeholder="sample@gmail.com"
                                value={inputMail} onInput={e => setInputMail(e.target.value)}
                                maxLength={100} />
                        </div>
                    </div>

                    <button className="btn btn-secondary btn-sm mt-2" onClick={() => { submitPurchaseCode() }}>Get Activation Code</button>

                    <button className="btn btn-outline-secondary btn-sm mt-1" onClick={logOut}>Log out!</button>

                </>);
            }
            else {
                _content = (<>
                    <p className="mt-2">Sign in to your envato account to confirm
                    that You are the true owner of extension</p>
                    <button type="submit" className="btn btn-signin-envato col-sm-10 col-md-8 col-lg-6 col-8 col-xsm-10 mt-3 ml-auto mr-auto"
                        style={{ backgroundImage: 'url("' + envatoicon + '")', maxWidth: '240px' }} onClick={gotoEnvato} >Sign in with Envato</button>
                    <p className="mt-3">Ater login you will redirect to this page again to continue activation!</p>
                    <ul className="text-left">
                        <li>Prepare your Purchase code that Envato market e-mailed you fo buy this extension</li>
                        <li>Prepare your e-mail address that you registered in Envato market</li>
                        <li>If you do not know how to find Purchase code follow this tutorial!</li>
                        <li>This registration need to keep your purchase code saprevent
                        You can use this extension on two machines at the same time.
                        Every activation Code just activate one client
                        so you need to get a uniqe activation code for each of your clients</li>
                    </ul>
                </>);
            }
        }
        else {
            _content = (<>
                {authenticationResult?.boolResult ? <> <h3 className="mt-2 text-light" >Congrats!</h3>
                <h5 className="mt-2 accent-color" >Attention!</h5>
                <h5 className="mt-2 accent-color" >Copy this Code into extension and activate it!</h5> </>:
                 <> <h5 className="mt-2 accent-color" >Attention!</h5>
                <h5 className="mt-2 accent-color" >Something is wrong!</h5></>   }
                <div className="bg-light w-100 mt-3">
                    <h5 className="mt-2 accent-color text-dark text-center" >{authenticationResult.description}</h5>
                </div>
                <p className="mt-3 pl-2 m-0 text-left">You can only activate ONE extension with this code!</p>
                <p className="pl-2 m-0 text-left">- You are responsible for keeping this code safe to prevent unauthorized copying of the program!</p>
                <button className="mt-3 btn btn-outline-secondary " onClick={logOut}>Exit</button>
                <br />
                {activeSessions && activeSessions.length > 0 ? <p className="pl-2 m-0 text-left">Current active seasons for this purchase code :</p> : null}

                {activeSessions && activeSessions.length > 0 ?

                    activeSessions.map((item) => {
                        return <div className="seasion-container p-1 m-1 d-flex border rounded" key={item.Id}>
                            <img src={logo} width="80" height="80" className="d-inline-block align-top p-2 bg-dark" alt="" />
                            <div className="seasion-container pt-0 m-0 d-flex flex-column flex-grow-1">
                                <p className="pl-2 m-0 text-left">{item.extenstion_name} ver. {item.extenstion_version}</p>
                                <p className="pl-2 m-0 text-left">User :{item.machine_name}</p>
                                <p className="pl-2 m-0 text-left btn-outline-secondary">activated in {item.application_name} version {item.application_version}</p>
                                <p className="pl-2 m-0 text-left btn-outline-secondary">activated at {new Date(item.activated_at).toDateString()}</p>
                            </div>
                            <button className="btn btn-sm text-warning" onClick={() => { deActivateSeason(item.Id) }}>Deactivate!</button>
                        </div>
                    })

                    : null}
            </>);
        }
        setContent(_content);
    }, [isAuthenticate, inputCode, inputMail, showActivaionCode, authenticationResult, activeSessions]);

    return (<div className="d-flex flex-column  text-left col-sm-12 col-md-10 col-lg-10 col-10 overflow-auto m-auto"
            style={{ maxWidth: '510px' }}>
            {content}
        </div>);

}

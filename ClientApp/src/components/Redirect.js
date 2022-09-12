import React, { useEffect } from 'react';
import Cookies from 'universal-cookie';
import { useLocation } from 'react-router-dom';

const cookies = new Cookies();

export default function Redirect(props) {

    const { search } = useLocation();

    useEffect(() => {

        const setuserCodeinCookie = (code) => {
            cookies.set('code', code, { path: '/', maxAge: 60 });
        }

        var codevals = new URLSearchParams(search);
        var _code = codevals.get("code");
        if (_code && _code.length > 0) {
            setuserCodeinCookie(_code);
        }
        props.history.push("/verify",);
    }, [search]);

    return (<div>
    </div>);

}

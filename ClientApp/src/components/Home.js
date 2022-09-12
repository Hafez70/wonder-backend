import React from 'react';
import imgPack from '../images/imgPack.jpeg';
import features from '../images/features.jpeg';
import afterEffect from '../images/ae.png';

export default function Home(props) {

    return (
        <div className="text-light d-flex flex-column overflow-auto">
            <div className="text-light flex-column flex-shrink-1">
                <h2 >Wonder Tools</h2>
                <h4 >Art and Tech Integration</h4>
                <ul>
                    <li>You can find our new product <a href='https://videohive.net/user/wondertools'>Wonder Call-Outs</a> in our page in Envato market</li>
                    <li>also You can find awesome tutorials inside the <a href='https://www.youtube.com/channel/UCNRVLqoNOfqFfNYTPFaEUYA'>Youtube channel</a> to learn more and more</li>
                    <li>if you got any question or problem please contact us with <strong>wondertools.ae@gmail.com</strong></li>
                </ul>
            </div>
            <div className="text-light flex-column flex-grow-1 flex-nowrap ">
                <div className="d-flex flex-sm-row flex-column p-3 col">
                    <div className="col-12 pt-5 pb-5 col-sm-12 col-md-4 col-lg-3 d-flex flex-column justify-content-center text-center flex-grow-0">
                        <img src={afterEffect} className="flex-column align-self-center" alt="after effects"
                            style={{ maxWidth: "150px", maxHeight: "150px" }} />
                        <h2>2015 - 2022</h2>
                        <h2>Extension</h2>
                    </div>
                    <iframe
                        className="col-12 col-sm-12 col-md-4 col-lg-6 d-flex pr-2 pl-2 flex-grow-1"
                        src={"https://www.youtube.com/embed/dkH4e914N34"}
                        title="Wonder Call-Outs"
                        frameBorder="0"
                        allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                        allowFullScreen></iframe>
                    <div className="col-12 pt-5 pb-5 col-sm-12 col-md-4 col-lg-3 d-flex flex-column justify-content-center text-center flex-grow-0">
                        <h2>Titles</h2>
                        <h2>Pop-up</h2>
                        <h2>Call-Outs</h2>
                    </div>
                   
                </div>
                <div className="d-flex flex-column">
                    <img src={imgPack} className="p-3" alt="Wonder-CallOut pkg" />
                    <img src={features} className="mt-2 p-3" alt="Features" />
                </div>
            </div>
        </div>
    );
}

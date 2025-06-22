"use client"

import { useEffect, useState } from 'react';
import { loadStripe } from '@stripe/stripe-js';
import { Elements, CardElement, useStripe, useElements } from '@stripe/react-stripe-js';
import axios from 'axios';
import {useSession} from "next-auth/react";

const stripePromise = loadStripe('pk_test_51Rac5bCmqDLurcRB80G7hdZXzeXu3mWryQQQh6befYxQ8lKCQRA5h2Elh0mNO50KKkPwTkTyopl2GzZww6bNg7QD00vtt31GYR');

const CheckoutForm = () => {
    const user = useSession()
    console.log(user)

    const stripe = useStripe();
    const elements = useElements();
    const [clientSecret, setClientSecret] = useState<string | null>(null);
    const [status, setStatus] = useState('');

    useEffect(() => {
    axios.get(`http://localhost:5119/billing/price_1RcRXtCmqDLurcRBv5reQchc`, {
        headers: {
            Authorization: `Bearer ${user.data?.accessToken}`,
        }
    }) // 👈 настрой proxy или замену
            .then(res => {
                console.log("ASDASDAS")
                setClientSecret(res.data.client_secret)
                console.log('ClientSecret', res.data.client_secret)
            })
            .catch(err => console.error(err));
    }, [user.data?.sub]);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!stripe || !elements || !clientSecret) return;

        console.log(clientSecret)


        try {
            const result = await stripe.confirmCardPayment(clientSecret, {
                payment_method: {
                    card: elements.getElement(CardElement)!
                }
            });

            if (result.error) {
                setStatus(`Ошибка: ${result.error.message}`);
            } else if (result.paymentIntent?.status === 'succeeded') {
                setStatus('✅ Подписка оформлена!');
            }
        }
        catch (error) {
            console.log(error);
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <CardElement options={
                {
                    style: {
                        base: {
                            fontSize: '16px',
                            color: '#a6a6d5',
                            '::placeholder': {
                                color: '#aab7c4',
                            },
                            fontFamily: 'Roboto, Open Sans, Segoe UI, sans-serif',
                            padding: '12px 16px',
                        },
                        invalid: {
                            color: '#fa755a',
                            iconColor: '#fa755a',
                        },
                    }
                }
            } />
            <button disabled={!stripe || !clientSecret}>Подписаться</button>
            <p>{status}</p>
        </form>
    );
};

export default function Home() {
    return (
        <Elements stripe={stripePromise}>
            <div className={"p-5 text-white"}>
                <h1>Оформить подписку</h1>
                <CheckoutForm />
            </div>
        </Elements>
    );
}

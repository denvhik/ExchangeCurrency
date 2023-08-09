import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ExchangeServiceService {

  private apiUrl = 'https://localhost:44334/api/CurrencyConversion';
  
  constructor(private http: HttpClient) { }

  async convertCurrency(sourceCurrency: string, amount: number, targetCurrency: string): Promise<number> {
    const body = {
      sourceCurrency: sourceCurrency,
      amount: amount,
      targetCurrency: targetCurrency
    };
    
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    const response = await this.http.post<any>(this.apiUrl, body, { headers }).toPromise();
    const exchangeRates = response;
    console.log (response)

    sourceCurrency = sourceCurrency.toUpperCase();
    targetCurrency = targetCurrency.toUpperCase();

    if (exchangeRates.hasOwnProperty(sourceCurrency) && exchangeRates.hasOwnProperty(targetCurrency)) {
      const sourceCurrencyRate = exchangeRates[sourceCurrency];
      const targetCurrencyRate = exchangeRates[targetCurrency];

      const convertedAmount = (amount / sourceCurrencyRate) * targetCurrencyRate;
     console.log (convertedAmount)
     console.log (response)
      return convertedAmount;

    }

    throw new Error('Invalid currency codes.');
  }
}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Reward } from '../models/reward.model';


@Injectable({ providedIn: 'root' })
export class RewardService {
  private apiUrl = '/api/rewards';

  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get<Reward[]>(this.apiUrl);
  }
  getById(id: number) {
    return this.http.get<Reward>(`${this.apiUrl}/${id}`);
  }


  create(data: Reward) {
    return this.http.post(this.apiUrl, data);
  }

  update(id: number, data: Reward) {
    return this.http.put(`${this.apiUrl}/${id}`, data);
  }

  delete(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}

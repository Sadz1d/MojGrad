import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { Reward } from '../../models/reward.model';
import { RewardService } from '../../services/reward.service';

@Component({
  selector: 'app-reward-list',
  templateUrl: './reward-list.component.html',
  styleUrls: ['./reward-list.component.scss']
})

export class RewardListComponent implements OnInit {
  rewards: Reward[] = [];

  constructor(
    private rewardService: RewardService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadRewards();
  }

  loadRewards() {
    this.rewardService.getAll().subscribe(res => {
      this.rewards = res;
    });
  }

  edit(id: number) {
    this.router.navigate(['/rewards', id, 'edit']);
  }

  delete(id: number) {
    if (confirm('Obrisati nagradu?')) {
      this.rewardService.delete(id).subscribe(() => {
        this.loadRewards();
      });
    }
  }
}

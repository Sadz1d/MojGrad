// Lista (GET /api/volunteering/actions)
export interface VolunteerActionListItem {
  id: number;
  name: string;
  description: string;
  location: string;
  eventDate: Date;
  maxParticipants: number;
  participantsCount: number;
  freeSlots: number;

  isUserJoined?: boolean;

}

// Kreiranje (ADMIN)
export interface CreateVolunteerActionCommand {
  name: string;
  description: string;
  location: string;
  eventDate: Date;
  maxParticipants: number;
}

// Update (ADMIN)
export interface UpdateVolunteerActionCommand {
  name?: string;
  description?: string;
  location?: string;
  eventDate?: Date;
  maxParticipants?: number;
}

// Paginacija (isti fazon kao kod problema)
export interface PageResult<T> {
  items: T[];
  total: number;
}

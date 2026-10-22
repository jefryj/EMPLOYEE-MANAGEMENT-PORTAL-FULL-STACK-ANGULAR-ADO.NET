import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Project, ProjectDto } from '../models/project';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private apiUrl = 'http://localhost:5187/api/projects';

  constructor(private http: HttpClient) { }

  getProjects(): Observable<Project[]> {
    const token = localStorage.getItem('token');

    return this.http.get<Project[]>(this.apiUrl, {
      headers: { Authorization: `Bearer ${token}` }
    });
  }

  addProject(project: ProjectDto): Observable<string> {
    const token = localStorage.getItem('token');

    return this.http.post<string>(this.apiUrl, project, {
      headers: { Authorization: `Bearer ${token}` }
    });
  }

  updateProject(id: number, project: ProjectDto): Observable<string> {
    const token = localStorage.getItem('token');

    return this.http.put<string>(`${this.apiUrl}/${id}`, project, {
      headers: { Authorization: `Bearer ${token}` }
    });
  }

  deleteProject(id: number): Observable<string> {
    const token = localStorage.getItem('token');

    return this.http.delete<string>(`${this.apiUrl}/${id}`, {
      headers: { Authorization: `Bearer ${token}` }
    });
  }
}
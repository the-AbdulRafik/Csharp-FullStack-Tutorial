import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import {
  Project,
  SkillGroup,
  ExperienceItem,
  EducationItem,
  AchievementItem,
  PortfolioStats
} from '../../models/portfolio.models';

@Injectable({
  providedIn: 'root'
})
export class PortfolioDataService {
  private readonly http = inject(HttpClient);

  readonly profile = {
    name: 'Abdul Rafik S',
    headline: 'AI Backend Engineer',
    email: 'abdulrafiks.106@gmail.com',
    phone: '+91 000000000',
    location: 'Tamil Nadu, India',
    github: 'https://github.com',
    linkedin: 'https://linkedin.com',
    summary:
      'AI Backend Engineer who transforms complex problems into intelligent, production-ready systems. Builds end-to-end solutions spanning AI workflows, scalable backend infrastructure, and integrated applications, with a focus on reliability, strong engineering, and real-world impact.'
  };

  private readonly projects: Project[] = [
    {
      id: 'autonomous-red-teaming-agent',
      title: 'Autonomous Red Teaming Agent',
      year: 2026,
      category: 'AI & Agents',
      summary:
        'A six-node LangGraph workflow for configurable, multi-turn LLM security testing using user-selected vulnerabilities and relevant MITRE ATLAS techniques.',
      technologies: ['LangGraph', 'LangChain', 'Python', 'FastAPI', 'LLM Evaluation'],
      highlights: [
        'Built a six-node LangGraph workflow for configurable, multi-turn LLM security testing using user-selected vulnerabilities and relevant MITRE ATLAS techniques.',
        'Implemented a Critic–Finalizer revision loop and adaptive replanning after refusals.',
        'Observed protected-configuration extraction within 7–6 turns in tested scenarios.'
      ],
      featured: true,
      githubUrl: 'https://github.com'
    },
    {
      id: 'pstack-antigravity',
      title: 'pstack-antigravity',
      year: 2026,
      category: 'Agent Runtime',
      summary:
        'Architected and published a native Antigravity port of Lauren Tan’s playbooks featuring 65 skills and 23 playbooks with strict token budgets.',
      technologies: ['Node.js', 'JavaScript', 'Google Antigravity Agent Runtime', 'CI/CD'],
      highlights: [
        'Architected and published a native Antigravity port of Lauren Tan’s playbooks: 65 skills, 23 playbooks.',
        'Enforced a strict 20,000-token rules budget using hierarchical progressive disclosure and AST token tracking.',
        'Designed non-recursive leaf subagent DAG manifests to prevent execution loops and reduce token burn.',
        'Built an automated behavioral eval harness verifying audit gates, TDD reproductions and prose linting.',
        'Distributed via an npm CLI with cross-platform directory sync and zero-dependency runtime scripts.'
      ],
      featured: true,
      githubUrl: 'https://github.com'
    },
    {
      id: 'reshare-hub',
      title: 'Reshare Hub',
      year: 2025,
      category: 'Full-Stack',
      summary:
        'A cross-platform React Native app connecting donors and recipients across 3 verified roles with Firestore 10 km location filtering and AI-assisted listings.',
      technologies: ['React Native', 'Flask', 'Firebase', 'Gemini API', 'VADER'],
      highlights: [
        'Built a cross-platform React Native app connecting donors and recipients across 3 verified roles, with Firestore-based 10 km location filtering.',
        'Developed Flask REST APIs and Firebase services for donor–recipient matching across 10+ categories, with AI-assisted listing generation and photo screening.',
        'Calculated donor trust scores using VADER sentiment analysis on recipient feedback.'
      ],
      featured: true,
      githubUrl: 'https://github.com'
    }
  ];

  private readonly skillGroups: SkillGroup[] = [
    {
      category: 'Languages',
      skills: ['Python', 'Java', 'TypeScript', 'JavaScript']
    },
    {
      category: 'AI & Agentic Frameworks',
      skills: [
        'LangGraph',
        'LangChain',
        'OpenAI SDK',
        'LiteLLM',
        'Strands Agents',
        'MCP',
        'RAG',
        'Tool Calling',
        'LangSmith',
        'LLM Evaluation'
      ]
    },
    {
      category: 'LLM APIs',
      skills: ['Gemini', 'Groq', 'OpenRouter', 'DeepSeek', 'NVIDIA NIM', 'Ollama']
    },
    {
      category: 'Backend Frameworks',
      skills: ['FastAPI', 'Flask', 'Express.js', 'Node.js', 'Csharp', '.Net', 'REST APIs']
    },
    {
      category: 'Infrastructure & Data',
      skills: [
        'Docker',
        'Microservices',
        'SQL (PostgreSQL, MySQL)',
        'NoSQL (MongoDB, Firestore)'
      ]
    },
    {
      category: 'Frontend & Tools',
      skills: ['React', 'React Native', 'Expo', 'Tailwind CSS', 'Git', 'GitHub', 'Postman']
    }
  ];

  private readonly experience: ExperienceItem[] = [
    {
      role: 'AI/ML Engineer Intern',
      company: 'Giggso Pvt Ltd',
      location: 'Chennai, India',
      period: 'May 2026 – Aug 2026',
      highlights: [
        'Built a proof of concept for handwritten document extraction and translation: a 2-stage Gemini pipeline decoupling parsing from translation, reaching 80% extraction accuracy on 20 handwritten test samples.',
        'Worked on an R&D team migrating a legacy Java platform to Python: mapped Kafka event streams and existing workflows onto Temporal workflows and Kafka for 2 core microservices.'
      ]
    }
  ];

  private readonly education: EducationItem[] = [
    {
      degree: 'B.Tech in Artificial Intelligence and Data Science',
      institution: 'PSNA College of Engineering and Technology',
      university: 'Anna University',
      location: 'Dindigul, Tamil Nadu',
      period: '2023 – 2027',
      score: 'CGPA: 8.70/10'
    }
  ];

  private readonly achievements: AchievementItem[] = [
    {
      award: 'Grand Prize Winner',
      competition: 'Code-O-Clock 24-Hour Hackathon (75+ teams)',
      details: 'Reshare Hub, built as D-Waste',
      date: 'Oct 2024'
    },
    {
      award: 'State Hackathon Winner',
      competition: 'EDII State-Level Hackathon',
      details: 'ArtoCraphy',
      date: 'Nov 2024'
    },
    {
      award: 'Third Prize',
      competition: 'HackXelerate Hackathon (100+ teams)',
      details: 'Reshare Hub',
      date: 'Apr 2025'
    }
  ];

  getProjects(): Observable<Project[]> {
    return of(this.projects);
  }

  getProjectById(id: string): Observable<Project | undefined> {
    const match = this.projects.find((p) => p.id === id);
    return of(match);
  }

  getFeaturedProjects(): Observable<Project[]> {
    return of(this.projects.filter((p) => p.featured));
  }

  getSkillGroups(): Observable<SkillGroup[]> {
    return of(this.skillGroups);
  }

  getExperience(): Observable<ExperienceItem[]> {
    return of(this.experience);
  }

  getEducation(): Observable<EducationItem[]> {
    return of(this.education);
  }

  getAchievements(): Observable<AchievementItem[]> {
    return of(this.achievements);
  }

  fetchLiveStats(): Observable<PortfolioStats> {
    return this.http.get<PortfolioStats>('assets/portfolio-stats.json');
  }
}

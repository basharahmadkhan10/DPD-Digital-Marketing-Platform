using DPDDigitalMarketing.Data;

namespace DPDDigitalMarketing.Services
{
    public class DigitalLibraryService : IDigitalLibraryService
    {
        private readonly AppDbContext _context;

        public DigitalLibraryService(AppDbContext context)
        {
            _context = context;
        }

        public DigitalIssue? GetDigitalIssue(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null) return null;

            return BuildDigitalIssue(product.Id, product.Title, product.Category, product.Language, product.CoverImageUrl);
        }

        public List<DigitalIssue> GetAllDigitalIssues()
        {
            var products = _context.Products.Where(p => p.IsActive).OrderBy(p => p.Id).ToList();
            return products.Select(p => BuildDigitalIssue(p.Id, p.Title, p.Category, p.Language, p.CoverImageUrl)).ToList();
        }

        private DigitalIssue BuildDigitalIssue(int id, string title, string category, string language, string coverUrl)
        {
            var issue = new DigitalIssue
            {
                ProductId = id,
                Title = title,
                Category = category,
                Language = language,
                CoverImageUrl = coverUrl,
                IssueNumber = $"Vol. {id + 50} • Issue {id * 4}",
                ReleaseDate = DateTime.UtcNow.AddDays(-id * 2)
            };

            if (title.Contains("Defence") || title.Contains("Armed Forces"))
            {
                issue.Sections = BuildDefenceSpecial();
            }
            else if (title.Contains("Engineering") || title.Contains("PSU"))
            {
                issue.Sections = BuildEngineeringSpecial();
            }
            else if (title.Contains("Banking") || title.Contains("Financial"))
            {
                issue.Sections = BuildBankingSpecial();
            }
            else if (title.Contains("Rozgar") && (language == "Hindi" || title.Contains("Hindi")))
            {
                issue.Sections = BuildRozgarHindi();
            }
            else if (title.Contains("Rozgar") && (language == "Urdu" || title.Contains("Urdu")))
            {
                issue.Sections = BuildRozgarUrdu();
            }
            else if (title.Contains("Kurukshetra"))
            {
                issue.Sections = BuildKurukshetra();
            }
            else if (title.Contains("Yojana"))
            {
                issue.Sections = BuildYojana();
            }
            else if (title.Contains("Aajkal"))
            {
                issue.Sections = BuildAajkalUrdu();
            }
            else
            {

                issue.Sections = BuildEmploymentNewsGeneral();
            }

            return issue;
        }

        private List<IssueSection> BuildEmploymentNewsGeneral()
        {
            return new List<IssueSection>
            {
                new IssueSection
                {
                    Id = "overview",
                    Title = "1. Vacancy Highlights & Summary Matrix",
                    Duration = "4 min read",
                    Icon = "bi-card-checklist",
                    HtmlContent = @"
                        <div class='mb-4'>
                            <div class='alert alert-primary border-0 rounded-4 p-3 d-flex align-items-center gap-3 mb-4'>
                                <i class='bi bi-megaphone-fill fs-3 text-primary'></i>
                                <div>
                                    <strong>Gazette Notification Alert:</strong> This week's official edition notifies
                                    <strong>28,450+ vacancies</strong> across Ministries, SSC, Defence, Banking, and PSUs.
                                </div>
                            </div>

                            <h4 class='fw-bold mb-3'>Major Central & State Recruitment Drives</h4>
                            <div class='table-responsive'>
                                <table class='table table-bordered align-middle'>
                                    <thead class='table-light'>
                                        <tr>
                                            <th>Organization</th>
                                            <th>Post Name</th>
                                            <th>Total Vacancies</th>
                                            <th>Eligibility</th>
                                            <th>Last Date</th>
                                            <th>Portal</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td class='fw-bold'>Staff Selection Commission (SSC)</td>
                                            <td>Combined Graduate Level (CGL 2026)</td>
                                            <td><span class='badge bg-success rounded-pill px-3 py-1'>14,500</span></td>
                                            <td>Bachelor's Degree (Any Discipline)</td>
                                            <td>15 Dec 2026</td>
                                            <td><span class='badge bg-dark rounded-pill'>ssc.gov.in</span></td>
                                        </tr>
                                        <tr>
                                            <td class='fw-bold'>Union Public Service Commission (UPSC)</td>
                                            <td>Civil Services Examination (CSE 2027)</td>
                                            <td><span class='badge bg-success rounded-pill px-3 py-1'>1,150</span></td>
                                            <td>Graduate (Age 21-32)</td>
                                            <td>05 Jan 2027</td>
                                            <td><span class='badge bg-dark rounded-pill'>upsc.gov.in</span></td>
                                        </tr>
                                        <tr>
                                            <td class='fw-bold'>Railway Recruitment Board (RRB)</td>
                                            <td>Non-Technical Categories (NTPC)</td>
                                            <td><span class='badge bg-success rounded-pill px-3 py-1'>8,200</span></td>
                                            <td>12th / Graduate</td>
                                            <td>20 Dec 2026</td>
                                            <td><span class='badge bg-dark rounded-pill'>rrbcdg.gov.in</span></td>
                                        </tr>
                                        <tr>
                                            <td class='fw-bold'>Institute of Banking Personnel (IBPS)</td>
                                            <td>Probationary Officers (PO XIV)</td>
                                            <td><span class='badge bg-success rounded-pill px-3 py-1'>4,600</span></td>
                                            <td>Degree with 55% marks</td>
                                            <td>18 Dec 2026</td>
                                            <td><span class='badge bg-dark rounded-pill'>ibps.in</span></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>"
                },
                new IssueSection
                {
                    Id = "eligibility",
                    Title = "2. Eligibility, Age Relaxation & DoPT Norms",
                    Duration = "5 min read",
                    Icon = "bi-person-badge",
                    HtmlContent = @"
                        <div>
                            <h4 class='fw-bold mb-3'>Standard Age Relaxation & Certificate Guidelines</h4>
                            <p class='text-muted'>Central Government reservation and relaxation guidelines as established by DoPT:</p>
                            <ul class='list-group list-group-flush mb-4'>
                                <li class='list-group-item d-flex justify-content-between align-items-center'>
                                    <span><strong>Scheduled Castes / Scheduled Tribes (SC/ST):</strong></span>
                                    <span class='badge bg-primary-subtle text-primary rounded-pill'>5 Years Upper Age Relaxation</span>
                                </li>
                                <li class='list-group-item d-flex justify-content-between align-items-center'>
                                    <span><strong>Other Backward Classes (OBC - Non-Creamy Layer):</strong></span>
                                    <span class='badge bg-primary-subtle text-primary rounded-pill'>3 Years Upper Age Relaxation</span>
                                </li>
                                <li class='list-group-item d-flex justify-content-between align-items-center'>
                                    <span><strong>Persons with Benchmark Disabilities (PwBD):</strong></span>
                                    <span class='badge bg-primary-subtle text-primary rounded-pill'>10 Years (General) / 15 Years (SC/ST)</span>
                                </li>
                            </ul>
                        </div>"
                },
                new IssueSection
                {
                    Id = "syllabus",
                    Title = "3. Exam Pattern & Syllabus Schemes",
                    Duration = "7 min read",
                    Icon = "bi-mortarboard",
                    HtmlContent = @"
                        <div>
                            <h4 class='fw-bold mb-3'>SSC CGL & RRB Examination Schemes</h4>
                            <div class='row g-3 mb-4'>
                                <div class='col-md-6'>
                                    <div class='p-3 rounded-4 border bg-light h-100'>
                                        <h5 class='fw-bold text-primary mb-2'>SSC CGL Tier-I Scheme</h5>
                                        <p class='small text-muted mb-2'>CBT (100 Questions • 200 Marks • 60 Mins)</p>
                                        <ul class='small mb-0'>
                                            <li>Reasoning: 25 Qs (50 Marks)</li>
                                            <li>General Awareness: 25 Qs (50 Marks)</li>
                                            <li>Quantitative Aptitude: 25 Qs (50 Marks)</li>
                                            <li>English Comprehension: 25 Qs (50 Marks)</li>
                                            <li><strong>Negative Marking:</strong> 0.50 marks per wrong answer</li>
                                        </ul>
                                    </div>
                                </div>
                                <div class='col-md-6'>
                                    <div class='p-3 rounded-4 border bg-light h-100'>
                                        <h5 class='fw-bold text-success mb-2'>UPSC Civil Services Prelims</h5>
                                        <p class='small text-muted mb-2'>OMR Based (2 Papers • 400 Total Marks)</p>
                                        <ul class='small mb-0'>
                                            <li>Paper I (General Studies): 100 Qs • 200 Marks</li>
                                            <li>Paper II (CSAT): 80 Qs • 200 Marks (Qualifying 33%)</li>
                                            <li>Negative Marking: 1/3rd penalty</li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>"
                },
                new IssueSection
                {
                    Id = "editorial",
                    Title = "4. Editorial: Civil Services Careers in the AI Era",
                    Duration = "5 min read",
                    Icon = "bi-feather",
                    HtmlContent = @"
                        <article>
                            <h4 class='fw-bold mb-3'>The Digital Evolution of Public Administration</h4>
                            <p class='text-muted'><em>By Dr. K. Radhakrishnan, Senior Fellow in Public Policy</em></p>
                            <p style='line-height: 1.8;'>
                                Government examinations today test real-world policy formulation, data-driven governance, and ethical public administration. As initiatives like IndiaAI and Digital Public Infrastructure (DPI) expand, administrative aspirants must demonstrate familiarity with technology governance.
                            </p>
                        </article>"
                },
                new IssueSection
                {
                    Id = "download",
                    Title = "5. Official Portals & Full Gazette PDF",
                    Duration = "2 min read",
                    Icon = "bi-download",
                    HtmlContent = @"
                        <div>
                            <h4 class='fw-bold mb-3'>Official Recruitment Portals</h4>
                            <div class='d-flex flex-wrap gap-2 mb-4'>
                                <a href='https://upsc.gov.in' target='_blank' class='btn btn-ink btn-sm'>upsc.gov.in</a>
                                <a href='https://ssc.gov.in' target='_blank' class='btn btn-ink btn-sm'>ssc.gov.in</a>
                                <a href='https://rrbcdg.gov.in' target='_blank' class='btn btn-ink btn-sm'>rrbcdg.gov.in</a>
                            </div>
                            <button type='button' class='btn btn-warning rounded-pill px-4 fw-bold text-dark' onclick='alert(""Official Gazette PDF download initiated."");'>
                                <i class='bi bi-file-earmark-pdf-fill me-1'></i> Download Complete Gazette PDF
                            </button>
                        </div>"
                }
            };
        }

        private List<IssueSection> BuildDefenceSpecial()
        {
            return new List<IssueSection>
            {
                new IssueSection
                {
                    Id = "defence-matrix",
                    Title = "1. Armed Forces Vacancy Highlights (12,800+ Posts)",
                    Duration = "5 min read",
                    Icon = "bi-shield-shaded",
                    HtmlContent = @"
                        <div>
                            <div class='alert alert-warning border-0 rounded-4 p-3 mb-4'>
                                <i class='bi bi-shield-fill-check me-2'></i>
                                <strong>Armed Forces Special Edition:</strong> Official notifications for Indian Army, Navy, Air Force, Coast Guard & CAPF.
                            </div>
                            <h4 class='fw-bold mb-3'>Officer & Non-Officer Entry Streams</h4>
                            <div class='table-responsive'>
                                <table class='table table-bordered align-middle'>
                                    <thead class='table-light'>
                                        <tr>
                                            <th>Branch / Force</th>
                                            <th>Exam / Entry Scheme</th>
                                            <th>Vacancies</th>
                                            <th>Qualification</th>
                                            <th>Age Limit</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td class='fw-bold'>Indian Army</td>
                                            <td>Combined Defence Services (CDS II) - IMA / OTA</td>
                                            <td><span class='badge bg-success rounded-pill px-3 py-1'>459</span></td>
                                            <td>Graduate (Any Stream)</td>
                                            <td>19 - 25 Yrs</td>
                                        </tr>
                                        <tr>
                                            <td class='fw-bold'>Indian Air Force</td>
                                            <td>AFCAT (Flying & Ground Duty Branches)</td>
                                            <td><span class='badge bg-success rounded-pill px-3 py-1'>317</span></td>
                                            <td>B.E./B.Tech or Any Degree with Maths & Physics</td>
                                            <td>20 - 24 Yrs</td>
                                        </tr>
                                        <tr>
                                            <td class='fw-bold'>Indian Navy</td>
                                            <td>Sailor SSR / MR (Agniveer Batch 01/2027)</td>
                                            <td><span class='badge bg-success rounded-pill px-3 py-1'>4,200</span></td>
                                            <td>10+2 with Maths & Physics / 10th Pass</td>
                                            <td>17.5 - 21 Yrs</td>
                                        </tr>
                                        <tr>
                                            <td class='fw-bold'>CAPF (CRPF, BSF, CISF, ITBP)</td>
                                            <td>Assistant Commandants (UPSC CAPF AC 2026)</td>
                                            <td><span class='badge bg-success rounded-pill px-3 py-1'>506</span></td>
                                            <td>Bachelor's Degree</td>
                                            <td>20 - 25 Yrs</td>
                                        </tr>
                                        <tr>
                                            <td class='fw-bold'>Indian Army Agniveer</td>
                                            <td>General Duty, Tradesmen, Clerk & Technical</td>
                                            <td><span class='badge bg-success rounded-pill px-3 py-1'>7,300</span></td>
                                            <td>10th / 12th Pass</td>
                                            <td>17.5 - 21 Yrs</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>"
                },
                new IssueSection
                {
                    Id = "ssb-interview",
                    Title = "2. 5-Day SSB Interview Procedure & Testing Protocol",
                    Duration = "7 min read",
                    Icon = "bi-diagram-3",
                    HtmlContent = @"
                        <div>
                            <h4 class='fw-bold mb-3'>Services Selection Board (SSB) Assessment Stages</h4>
                            <p class='text-muted'>The comprehensive 5-day evaluation procedure assessing 15 Officer Like Qualities (OLQs):</p>

                            <div class='row g-3 mb-4'>
                                <div class='col-md-6'>
                                    <div class='p-3 rounded-4 bg-light border'>
                                        <h6 class='fw-bold text-primary'>Stage 1: Screening Test (Day 1)</h6>
                                        <ul class='small mb-0'>
                                            <li><strong>OIR Test:</strong> Verbal & Non-Verbal Intelligence test</li>
                                            <li><strong>PPDT:</strong> Picture Perception & Description Test (30 sec view, 4 min story write, group discussion)</li>
                                            <li>Screened-in candidates advance to Stage 2.</li>
                                        </ul>
                                    </div>
                                </div>
                                <div class='col-md-6'>
                                    <div class='p-3 rounded-4 bg-light border'>
                                        <h6 class='fw-bold text-success'>Stage 2: Psychology & GTO (Days 2 - 4)</h6>
                                        <ul class='small mb-0'>
                                            <li><strong>TAT, WAT, SRT & SDT:</strong> Thematic Apperception, Word Association & Situation Reaction</li>
                                            <li><strong>GTO Ground Tasks:</strong> Progressive Group Task (PGT), Snake Race, Command Task, Lecturette</li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>"
                },
                new IssueSection
                {
                    Id = "physical-standards",
                    Title = "3. Physical & Medical Standards (Height, Vision, PET)",
                    Duration = "5 min read",
                    Icon = "bi-heart-pulse",
                    HtmlContent = @"
                        <div>
                            <h4 class='fw-bold mb-3'>Minimum Physical Endurance Requirements</h4>
                            <table class='table table-striped align-middle'>
                                <thead>
                                    <tr>
                                        <th>Test / Measurement</th>
                                        <th>Male Candidates</th>
                                        <th>Female Candidates</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td class='fw-bold'>Minimum Height</td>
                                        <td>157.5 cm (Army/Navy), 162.5 cm (Air Force Flying)</td>
                                        <td>152 cm</td>
                                    </tr>
                                    <tr>
                                        <td class='fw-bold'>Running Endurance</td>
                                        <td>1.6 km in 5 mins 30 secs - 6 mins 30 secs</td>
                                        <td>800m in 4 minutes</td>
                                    </tr>
                                    <tr>
                                        <td class='fw-bold'>Dips / Push-ups</td>
                                        <td>Minimum 10 - 15 Repetitions</td>
                                        <td>10 Sit-ups</td>
                                    </tr>
                                    <tr>
                                        <td class='fw-bold'>Visual Acuity</td>
                                        <td>6/6 (Unaided Flying), 6/9 Correctable to 6/6</td>
                                        <td>6/6 Correctable</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>"
                },
                new IssueSection
                {
                    Id = "defence-apply",
                    Title = "4. Direct Official Recruitment Gateways",
                    Duration = "2 min read",
                    Icon = "bi-box-arrow-up-right",
                    HtmlContent = @"
                        <div>
                            <h4 class='fw-bold mb-3'>Apply Online Directly</h4>
                            <div class='d-flex flex-wrap gap-2 mb-4'>
                                <a href='https://joinindianarmy.nic.in' target='_blank' class='btn btn-ink btn-sm'>joinindianarmy.nic.in</a>
                                <a href='https://joinindiannavy.gov.in' target='_blank' class='btn btn-ink btn-sm'>joinindiannavy.gov.in</a>
                                <a href='https://careerindianairforce.cdac.in' target='_blank' class='btn btn-ink btn-sm'>careerindianairforce.cdac.in</a>
                            </div>
                        </div>"
                }
            };
        }

        private List<IssueSection> BuildEngineeringSpecial()
        {
            return new List<IssueSection>
            {
                new IssueSection
                {
                    Id = "psu-matrix",
                    Title = "1. Maharatna & Navratna PSU Vacancies (8,400+ Posts)",
                    Duration = "5 min read",
                    Icon = "bi-gear-wide-connected",
                    HtmlContent = @"
                        <div>
                            <div class='alert alert-info border-0 rounded-4 p-3 mb-4'>
                                <i class='bi bi-cpu-fill me-2'></i>
                                <strong>Engineering & Technical Special:</strong> GATE recruitments, State DISCOMs, ISRO & BARC openings.
                            </div>
                            <h4 class='fw-bold mb-3'>Active PSU Recruitment Drives</h4>
                            <div class='table-responsive'>
                                <table class='table table-bordered align-middle'>
                                    <thead class='table-light'>
                                        <tr>
                                            <th>PSU Enterprise</th>
                                            <th>Post Designation</th>
                                            <th>Vacancies</th>
                                            <th>Eligible Disciplines</th>
                                            <th>Selection Basis</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td class='fw-bold'>Indian Oil (IOCL)</td>
                                            <td>Graduate Engineer Trainee (GET)</td>
                                            <td><span class='badge bg-success rounded-pill px-3 py-1'>1,420</span></td>
                                            <td>Mechanical, Chemical, Electrical, Civil</td>
                                            <td>GATE 2026 Score (85%) + GD/GT & PI (15%)</td>
                                        </tr>
                                        <tr>
                                            <td class='fw-bold'>NTPC Limited</td>
                                            <td>Executive Trainees (ET-2026)</td>
                                            <td><span class='badge bg-success rounded-pill px-3 py-1'>864</span></td>
                                            <td>Electrical, Mechanical, Instrumentation</td>
                                            <td>GATE Score + Personal Interview</td>
                                        </tr>
                                        <tr>
                                            <td class='fw-bold'>ISRO / Department of Space</td>
                                            <td>Scientist/Engineer 'SC'</td>
                                            <td><span class='badge bg-success rounded-pill px-3 py-1'>382</span></td>
                                            <td>B.Tech with 65% aggregate (ECE, ME, CS)</td>
                                            <td>ISRO Centralized Written Exam + Interview</td>
                                        </tr>
                                        <tr>
                                            <td class='fw-bold'>BHEL & PowerGrid</td>
                                            <td>Assistant Engineer / Trainee</td>
                                            <td><span class='badge bg-success rounded-pill px-3 py-1'>1,250</span></td>
                                            <td>Power Engineering, Civil, Electronics</td>
                                            <td>GATE Score + Merit List</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>"
                },
                new IssueSection
                {
                    Id = "technical-interview",
                    Title = "2. PSU Technical Interview & Group Task Preparation",
                    Duration = "6 min read",
                    Icon = "bi-tools",
                    HtmlContent = @"
                        <div>
                            <h4 class='fw-bold mb-3'>How PSU Technical Boards Assess Candidates</h4>
                            <p class='text-muted'>Technical interviews in Maharatna companies focus on core conceptual depth rather than formula recall:</p>
                            <ul class='list-group list-group-flush mb-4'>
                                <li class='list-group-item'><strong>Final Year Project Rigor:</strong> Practical feasibility, sensor integration, thermodynamics or FEA modeling.</li>
                                <li class='list-group-item'><strong>Industrial Standards & Safety:</strong> Knowledge of ISO standards, boiler regulations (IBR), and electric grid codes.</li>
                                <li class='list-group-item'><strong>Group Tasks (GT):</strong> Conflict resolution, engineering compromise, and team leadership without aggression.</li>
                            </ul>
                        </div>"
                }
            };
        }

        private List<IssueSection> BuildBankingSpecial()
        {
            return new List<IssueSection>
            {
                new IssueSection
                {
                    Id = "banking-matrix",
                    Title = "1. Public Sector Banks & Insurance Openings (14,200+ Posts)",
                    Duration = "5 min read",
                    Icon = "bi-bank",
                    HtmlContent = @"
                        <div>
                            <div class='alert alert-success border-0 rounded-4 p-3 mb-4'>
                                <i class='bi bi-cash-stack me-2'></i>
                                <strong>Banking & Financial Sector Special:</strong> IBPS, SBI, RBI, and LIC recruitments breakdown.
                            </div>
                            <h4 class='fw-bold mb-3'>Major Banking Recruitment Cycles</h4>
                            <div class='table-responsive'>
                                <table class='table table-bordered align-middle'>
                                    <thead class='table-light'>
                                        <tr>
                                            <th>Bank / Institution</th>
                                            <th>Role</th>
                                            <th>Vacancies</th>
                                            <th>Exam Structure</th>
                                            <th>Pay Scale</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td class='fw-bold'>Institute of Banking Personnel (IBPS)</td>
                                            <td>Probationary Officer (PO/MT XIV)</td>
                                            <td><span class='badge bg-success rounded-pill px-3 py-1'>4,600</span></td>
                                            <td>Prelims (100 M) + Mains (225 M) + Interview (100 M)</td>
                                            <td>₹36,000 - ₹63,840</td>
                                        </tr>
                                        <tr>
                                            <td class='fw-bold'>State Bank of India (SBI)</td>
                                            <td>Specialist Cadre Officers (IT, Risk, Law)</td>
                                            <td><span class='badge bg-success rounded-pill px-3 py-1'>1,840</span></td>
                                            <td>Professional Knowledge Exam + Interview</td>
                                            <td>₹48,170 - ₹78,230</td>
                                        </tr>
                                        <tr>
                                            <td class='fw-bold'>Reserve Bank of India (RBI)</td>
                                            <td>Grade 'B' Officers (General / DEPR / DSIM)</td>
                                            <td><span class='badge bg-success rounded-pill px-3 py-1'>290</span></td>
                                            <td>Phase-I Objective + Phase-II Descriptive + Interview</td>
                                            <td>₹55,200 - ₹99,750</td>
                                        </tr>
                                        <tr>
                                            <td class='fw-bold'>Life Insurance Corporation (LIC)</td>
                                            <td>Assistant Administrative Officer (AAO)</td>
                                            <td><span class='badge bg-success rounded-pill px-3 py-1'>1,120</span></td>
                                            <td>Prelims + Mains + Medical</td>
                                            <td>₹53,600 - ₹102,000</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>"
                },
                new IssueSection
                {
                    Id = "banking-notes",
                    Title = "2. High-Yield Banking & Economic Awareness Capsule",
                    Duration = "7 min read",
                    Icon = "bi-currency-rupee",
                    HtmlContent = @"
                        <div>
                            <h4 class='fw-bold mb-3'>Key Financial Terms for Mains & Interviews</h4>
                            <div class='row g-3 mb-4'>
                                <div class='col-md-6'>
                                    <div class='p-3 rounded-4 bg-light border'>
                                        <h6 class='fw-bold text-primary'>Monetary Policy & Rates</h6>
                                        <ul class='small mb-0'>
                                            <li><strong>Repo Rate:</strong> Rate at which RBI lends short-term money to commercial banks against government securities.</li>
                                            <li><strong>SDF (Standing Deposit Facility):</strong> Tool to absorb liquidity without collateral.</li>
                                            <li><strong>Basel III Norms:</strong> Capital Adequacy Ratio (CAR) minimum requirement of 11.5% for Indian banks.</li>
                                        </ul>
                                    </div>
                                </div>
                                <div class='col-md-6'>
                                    <div class='p-3 rounded-4 bg-light border'>
                                        <h6 class='fw-bold text-success'>Digital Banking Initiatives</h6>
                                        <ul class='small mb-0'>
                                            <li><strong>Central Bank Digital Currency (e-Rupee):</strong> Sovereign digital currency issued by RBI on blockchain.</li>
                                            <li><strong>Account Aggregator (AA) Framework:</strong> Consent-based financial data sharing system facilitating collateral-free credit.</li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>"
                }
            };
        }

        private List<IssueSection> BuildRozgarHindi()
        {
            return new List<IssueSection>
            {
                new IssueSection
                {
                    Id = "hindi-overview",
                    Title = "1. प्रमुख सरकारी नौकरियां एवं रिक्तियों का विवरण",
                    Duration = "5 min read",
                    Icon = "bi-journal-check",
                    HtmlContent = @"
                        <div>
                            <div class='alert alert-primary border-0 rounded-4 p-3 mb-4'>
                                <i class='bi bi-newspaper me-2'></i>
                                <strong>रोजगार समाचार - हिंदी साप्ताहिक:</strong> प्रकाशन विभाग, सूचना एवं प्रसारण मंत्रालय, भारत सरकार।
                            </div>
                            <h4 class='fw-bold mb-3'>इस सप्ताह की शीर्ष भर्तियां</h4>
                            <div class='table-responsive'>
                                <table class='table table-bordered align-middle'>
                                    <thead class='table-light'>
                                        <tr>
                                            <th>विभाग / आयोग</th>
                                            <th>पद का नाम</th>
                                            <th>कुल पद</th>
                                            <th>योग्यता</th>
                                            <th>अंतिम तिथि</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td class='fw-bold'>कर्मचारी चयन आयोग (SSC)</td>
                                            <td>संयुक्त स्नातक स्तरीय परीक्षा (CGL)</td>
                                            <td><span class='badge bg-success rounded-pill px-3 py-1'>14,500</span></td>
                                            <td>स्नातक (किसी भी विषय में)</td>
                                            <td>15 दिसंबर 2026</td>
                                        </tr>
                                        <tr>
                                            <td class='fw-bold'>रेलवे भर्ती बोर्ड (RRB)</td>
                                            <td>गैर-तकनीकी लोकप्रिय श्रेणियां (NTPC)</td>
                                            <td><span class='badge bg-success rounded-pill px-3 py-1'>8,200</span></td>
                                            <td>12वीं पास / स्नातक</td>
                                            <td>20 दिसंबर 2026</td>
                                        </tr>
                                        <tr>
                                            <td class='fw-bold'>केंद्रीय सशस्त्र पुलिस बल (CAPF)</td>
                                            <td>कांस्टेबल (जीडी) भर्ती</td>
                                            <td><span class='badge bg-success rounded-pill px-3 py-1'>26,146</span></td>
                                            <td>10वीं पास (उम्र 18-23 वर्ष)</td>
                                            <td>31 दिसंबर 2026</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>"
                },
                new IssueSection
                {
                    Id = "hindi-guidance",
                    Title = "2. आयु सीमा छूट, आरक्षण एवं प्रमाण-पत्र नियम",
                    Duration = "6 min read",
                    Icon = "bi-card-heading",
                    HtmlContent = @"
                        <div>
                            <h4 class='fw-bold mb-3'>कार्मिक एवं प्रशिक्षण विभाग (DoPT) दिशा-निर्देश</h4>
                            <p class='text-muted'>केंद्रीय सेवाओं में आरक्षित श्रेणियों के लिए निर्धारित आयु सीमा में छूट:</p>
                            <ul class='list-group list-group-flush mb-4'>
                                <li class='list-group-item d-flex justify-content-between'><span>अनुसूचित जाति / जनजाति (SC/ST):</span> <strong class='text-primary'>5 वर्ष की छूट</strong></li>
                                <li class='list-group-item d-flex justify-content-between'><span>अन्य पिछड़ा वर्ग (OBC - गैर-क्रीमी लेयर):</span> <strong class='text-primary'>3 वर्ष की छूट</strong></li>
                                <li class='list-group-item d-flex justify-content-between'><span>दिव्यांग अभ्यर्थी (PwBD):</span> <strong class='text-primary'>10 से 15 वर्ष की छूट</strong></li>
                            </ul>
                        </div>"
                }
            };
        }

        private List<IssueSection> BuildRozgarUrdu()
        {
            return new List<IssueSection>
            {
                new IssueSection
                {
                    Id = "urdu-overview",
                    Title = "1. Rozgar Samachar Urdu - Markazi Mulazmaten",
                    Duration = "5 min read",
                    Icon = "bi-file-earmark-text",
                    HtmlContent = @"
                        <div dir='ltr'>
                            <div class='alert alert-warning border-0 rounded-4 p-3 mb-4'>
                                <i class='bi bi-briefcase-fill me-2'></i>
                                <strong>Rozgar Samachar Urdu Edition:</strong> Publications Division, Ministry of Information & Broadcasting.
                            </div>
                            <h4 class='fw-bold mb-3'>Weekly Vacancies in Urdu Medium & Central Departments</h4>
                            <p class='text-muted'>Opportunities in Central Universities, Translation Bureaus, Maulana Azad National Urdu University (MANUU), and SSC Multitasking Staff.</p>
                            <div class='table-responsive'>
                                <table class='table table-bordered align-middle'>
                                    <thead class='table-light'>
                                        <tr>
                                            <th>Idara / Organization</th>
                                            <th>Ohda / Post Name</th>
                                            <th>Asamiyan / Vacancies</th>
                                            <th>Qabiliyat / Qualification</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td class='fw-bold'>Staff Selection Commission (SSC)</td>
                                            <td>Junior Translation Officer (JTO / SHT)</td>
                                            <td><span class='badge bg-success rounded-pill px-3 py-1'>420</span></td>
                                            <td>Master's in Hindi/English/Urdu</td>
                                        </tr>
                                        <tr>
                                            <td class='fw-bold'>MANUU & Central Universities</td>
                                            <td>Assistant Professor & Non-Teaching Posts</td>
                                            <td><span class='badge bg-success rounded-pill px-3 py-1'>315</span></td>
                                            <td>Post Graduation + NET/PhD</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>"
                }
            };
        }

        private List<IssueSection> BuildYojana()
        {
            return new List<IssueSection>
            {
                new IssueSection
                {
                    Id = "yojana-theme",
                    Title = "1. Focus Theme: India's Techade & Digital Governance",
                    Duration = "6 min read",
                    Icon = "bi-cpu",
                    HtmlContent = @"
                        <article>
                            <div class='alert alert-primary border-0 rounded-4 p-3 mb-4'>
                                <i class='bi bi-book-half me-2'></i>
                                <strong>Yojana Journal (English Edition):</strong> Flagship Socio-Economic Development Journal.
                            </div>
                            <h3 class='fw-bold font-display mb-3'>Architecting India's Technological Century</h3>
                            <p class='text-muted' style='line-height: 1.8;'>
                                <em>Lead Essay by Senior Economic Advisor, NITI Aayog</em>
                            </p>
                            <p style='line-height: 1.8;'>
                                India's approach to digital governance departs fundamentally from the closed corporate platforms of the West. Through open-standard <strong>Digital Public Goods (DPGs)</strong>—comprising Aadhaar identity, UPI payment rails, DigiLocker, and ONDC—India has onboarded over 900 million citizens to the formal digital economy at negligible transaction costs.
                            </p>
                        </article>"
                },
                new IssueSection
                {
                    Id = "yojana-mains",
                    Title = "2. Civil Services Mains GS-III Strategy & Model Answer",
                    Duration = "8 min read",
                    Icon = "bi-pencil-square",
                    HtmlContent = @"
                        <div>
                            <h4 class='fw-bold mb-3'>UPSC Mains Examination Framework</h4>
                            <div class='p-4 rounded-4 bg-light border mb-4'>
                                <h6 class='fw-bold text-danger mb-2'>Mains GS-III Question (15 Marks • 250 Words):</h6>
                                <p class='fst-italic text-dark mb-0'>
                                    'How has the Digital Public Infrastructure (DPI) model democratized entrepreneurship and financial inclusion in tier-2 and tier-3 Indian cities? Critically evaluate.'
                                </p>
                            </div>
                            <h6 class='fw-bold text-success mb-2'>Core Answer Dimensions:</h6>
                            <ul class='list-group list-group-flush mb-4'>
                                <li class='list-group-item'><strong>1. The Identity & Payments Rail:</strong> Direct Benefit Transfer (DBT) has plugged ₹2.7 Lakh Crore in leakages.</li>
                                <li class='list-group-item'><strong>2. Open Networks (ONDC):</strong> Breaking e-commerce duopolies by letting neighborhood kirana stores compete equally.</li>
                                <li class='list-group-item'><strong>3. Challenges Ahead:</strong> Cyber literacy disparities, data privacy under the DPDP Act 2023, and rural server latency.</li>
                            </ul>
                        </div>"
                }
            };
        }

        private List<IssueSection> BuildKurukshetra()
        {
            return new List<IssueSection>
            {
                new IssueSection
                {
                    Id = "kuru-theme",
                    Title = "1. Focus Theme: Agritech & Rural Transformation",
                    Duration = "6 min read",
                    Icon = "bi-tree",
                    HtmlContent = @"
                        <article>
                            <div class='alert alert-success border-0 rounded-4 p-3 mb-4'>
                                <i class='bi bi-patch-check-fill me-2'></i>
                                <strong>Kurukshetra Journal:</strong> Dedicated to Rural Development, Agriculture, and Panchayati Raj.
                            </div>
                            <h3 class='fw-bold font-display mb-3'>Natural Farming, FPOs & Decentralized Economy</h3>
                            <p style='line-height: 1.8;'>
                                Over 86% of Indian farmers are small and marginal landholders. Sustainable rural income hinges on aggregating these producers into <strong>Farmer Producer Organizations (FPOs)</strong> to secure bulk purchasing power for seeds and solar-powered micro-irrigation systems.
                            </p>
                            <div class='row g-3 my-4'>
                                <div class='col-md-4'>
                                    <div class='p-3 rounded-4 bg-light border text-center'>
                                        <div class='display-6 fw-bold text-primary font-display'>10,000+</div>
                                        <div class='text-muted small'>FPOs Formed & Registered</div>
                                    </div>
                                </div>
                                <div class='col-md-4'>
                                    <div class='p-3 rounded-4 bg-light border text-center'>
                                        <div class='display-6 fw-bold text-success font-display'>3.2 Lakh</div>
                                        <div class='text-muted small'>Villages Drone Mapped</div>
                                    </div>
                                </div>
                                <div class='col-md-4'>
                                    <div class='p-3 rounded-4 bg-light border text-center'>
                                        <div class='display-6 fw-bold text-warning font-display'>₹18,000 Cr</div>
                                        <div class='text-muted small'>Rural Property Credit Generated</div>
                                    </div>
                                </div>
                            </div>
                        </article>"
                }
            };
        }

        private List<IssueSection> BuildAajkalUrdu()
        {
            return new List<IssueSection>
            {
                new IssueSection
                {
                    Id = "aajkal-literary",
                    Title = "1. Aajkal Literary Essay: Classical & Modern Urdu Poetry",
                    Duration = "7 min read",
                    Icon = "bi-feather",
                    HtmlContent = @"
                        <article>
                            <div class='alert alert-danger border-0 rounded-4 p-3 mb-4'>
                                <i class='bi bi-award-fill me-2'></i>
                                <strong>Aajkal Urdu Magazine:</strong> Prestigious literary monthly published by Publications Division since 1942.
                            </div>
                            <h3 class='fw-bold font-display mb-3'>Urdu Shayari aur Hamari Tehzibi Riwayat</h3>
                            <p class='text-muted'><em>By Professor Shamim Hanfi, Renowned Urdu Critic & Scholar</em></p>
                            <p style='line-height: 1.8;'>
                                Aajkal has served as the custodian of India's composite Ganga-Jamuni tehzeeb for over eight decades. From the profound philosophy of Mirza Asadullah Khan Ghalib to the revolutionary verses of Faiz Ahmad Faiz and Sahir Ludhianvi, this edition presents critical evaluations of modern Urdu nazm.
                            </p>
                        </article>"
                }
            };
        }
    }
}

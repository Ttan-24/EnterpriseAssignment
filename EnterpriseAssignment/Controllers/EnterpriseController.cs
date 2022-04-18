
using EnterpriseAssignment.DatabaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAssignment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnterpriseController : Controller
    {

        ///////////////////////////// Set up /////////////////////////////
        
        private readonly trivia_dbContext context;
    
        public EnterpriseController(trivia_dbContext context)
        {
            this.context = context;
        }



        ///////////////////////////// Categories /////////////////////////////

        [HttpGet] // attribute
        [Route("categories")]
        public IActionResult GetCategories()
        {
            // Get categories
            List<Category> categories = context.Categories.ToList();

            // Return
            Dictionary<string, string> links = new Dictionary<string, string>();
            links.Add("create session", "https://localhost:44382/api/Enterprise/session/{username}/{idcategory}/{questioncount}/{idapp}");
            return Ok(new ApiResponse<List<Category>>(categories, links));
        }



        ///////////////////////////// Sessions /////////////////////////////

        [HttpPost]
        [Route("session/{username}/{idcategory}/{questioncount}/{idapp}")]
        public IActionResult CreateSession(string username, int idcategory, int questioncount, string idapp)
        {
            // Check for existing session
            List<Session> existingSessions = context.Sessions.Where(s => s.Name == username && s.IdCategory == idcategory).ToList();
            if (existingSessions.Count > 0)
            {
                return BadRequest("Error: session with username and category already exists.");
            }

            // Get database table
            var dbSessions = context.Sessions;
            var questions = context.Questions.Where(q => q.IdCategory == idcategory).ToList();
            var dbSessionQuestions = context.Sessionquestions;
    
            // Get answer from http
            Session mySession = new Session();
            mySession.Name = username;
            mySession.Score = 0;
            mySession.IdCategory = idcategory;
            mySession.CurrentQuestionIndex = 0;
            mySession.QuestionCount = questioncount;
            mySession.AppId = idapp;

            // Add to table
            dbSessions.Add(mySession);
    
            // Save changes
            context.SaveChanges();
    
            // Get session again now that it has a userId
            Session newSession = context.Sessions.Where(s => s.Name == username).ToList().First();

            // Shuffle question list
            Random random = new Random();
            List<Question> randomQuestionList = new List<Question>();
            while (questions.Count > 0)
            {
                // Get random index  
                int index = random.Next(questions.Count);

                // Move to randomised list
                randomQuestionList.Add(questions[index]);
                questions.RemoveAt(index);
            }
            questions = randomQuestionList;

            // Create questions for session
            List<Sessionquestion> sessionQuestions = new List<Sessionquestion>();
            float questionCount = MathF.Min(questions.Count, questioncount);
            for (int i = 0; i < questionCount; i++)
            {
                // Get question
                Question question = questions[i];

                // Create session question
                Sessionquestion sessionQuestion = new Sessionquestion();
                sessionQuestion.IdSession = newSession.IdSession;
                sessionQuestion.IdQuestion = question.IdQuestion;
                sessionQuestion.OrderIndex = i;
    
                // Add to list
                sessionQuestions.Add(sessionQuestion);
            }

            // Add new session questions
            foreach (Sessionquestion sessionQuestion in sessionQuestions)
            {
                dbSessionQuestions.Add(sessionQuestion);
            }
    
            // Save again
            context.SaveChanges();


            // Return
            Dictionary<string, string> links = new Dictionary<string, string>();
            links.Add("question", "https://localhost:44382/api/Enterprise/question/{idsession}");
            return Ok(new ApiResponse<Session>(mySession, links));
        }

        [HttpGet]
        [Route("session/{idsession}")]
        public IActionResult GetSession(int idsession)
        {
            // Get sessions
            Session session = context.Sessions.Where(s => s.IdSession == idsession).ToList().Single();

            // Return sessions
            Dictionary<string, string> links = new Dictionary<string, string>();
            links.Add("sessions", "https://localhost:44382/api/Enterprise/sessions");
            return Ok(new ApiResponse<Session>(session, links));
        }

        [HttpGet]
        [Route("sessions")]
        public IActionResult GetSessions()
        {
            // Get sessions
            List<Session> sessions = context.Sessions.ToList();

            // Return sessions
            Dictionary<string, string> links = new Dictionary<string, string>();
            links.Add("session", "https://localhost:44382/api/Enterprise/sessions/{idsession}");
            links.Add("scores", "https://localhost:44382/api/Enterprise/sessions/scores");
            return Ok(new ApiResponse<List<Session>>(sessions, links));
        }



        ///////////////////////////// Session Scores /////////////////////////////

        [HttpGet]
        [Route("score/{idsession}")]
        public IActionResult GetSessionScore(int idsession)
        {
            // Get session
            Session dbSession = context.Sessions.Where(session => session.IdSession == idsession).Single();

            // Get score
            int score = dbSession.Score;

            // Return score
            Dictionary<string, string> links = new Dictionary<string, string>();
            links.Add("scores", "https://localhost:44382/api/Enterprise/sessions/scores");
            return Ok(new ApiResponse<int>(score, links));
        }

        [HttpGet] // attribute
        [Route("sessions/scores")]
        public IActionResult GetHighScores() // Has more techniques
        {
            // Get tables
            List<SessionAsHighscore> dbSessions = context.Sessions.OrderByDescending(session => session.Score).Select(s => new SessionAsHighscore(s.AppId, s.Name, s.Score)).ToList();

            // Return
            Dictionary<string, string> links = new Dictionary<string, string>();
            links.Add("session", "https://localhost:44382/api/Enterprise/sessions");
            return Ok(new ApiResponse<List<SessionAsHighscore>>(dbSessions, links));
        }



        ///////////////////////////// Questions /////////////////////////////

        [HttpGet] // attribute
        [Route("question/{idsession}")]
        public IActionResult GetQuestion(int idsession)
        {
            // Get session
            Session dbSession;
            try
            {
                dbSession = context.Sessions.Where(session => session.IdSession == idsession).Single();
            }
            catch (InvalidOperationException)
            {
                return StatusCode(500);
            }

            // if db session == null {
            //return NotFound("couldnt fine dbsession");
              //  }

            // Return immediately if session is over
            if (dbSession.EndSession == true)
            {
                string response = "Session has ended. Your score is " + dbSession.Score;

                Dictionary<string, string> DictionaryLinks = new Dictionary<string, string>();
                DictionaryLinks.Add("score", "https://localhost:44382/api/Enterprise/score/{idsession}");
                return Ok(new ApiResponse<string>(response, DictionaryLinks));
            }

            // Get session question
            Sessionquestion sessionQuestion = context.Sessionquestions
                .Where(q => q.IdSession == dbSession.IdSession)
                .Where(q => q.OrderIndex == dbSession.CurrentQuestionIndex)
                .ToList().FirstOrDefault();

            // Get question
            Question question = context.Questions.Where(question => question.IdQuestion == sessionQuestion.IdQuestion).Single();

            // Get question text
            string text = question.Prompt;
            if (!question.HasPicture)
            {
                text += "\n A) " + question.AnswerA + "\n B) " + question.AnswerB + "\n C) " + question.AnswerC + "\n D) " + question.AnswerD;
            }

            // Return question
            Dictionary<string, string> links = new Dictionary<string, string>();
            links.Add("answer", "https://localhost:44382/api/Enterprise/questions/{idsession}/{answercharacter}");
            links.Add("hint", "https://localhost:44382/api/Enterprise/questions/{idsession}/question/textHint/{idsession}");
            return Ok(new ApiResponse<string>(text, links));
        }
    
        [HttpPut]
        [Route("questions/{idsession}/{answercharacter}")]
        public IActionResult AnswerQuestion(int idsession, string answercharacter)
        {
            // Get session
            Session dbSession = context.Sessions.Where(session => session.IdSession == idsession).FirstOrDefault();

            // Return immediately if session is over
            if (dbSession.EndSession == true)
            {
                string response = "Session has ended. Your score is " + dbSession.Score;

                Dictionary<string, string> links = new Dictionary<string, string>();
                links.Add("score", "https://localhost:44382/api/Enterprise/score/{idsession}");
                links.Add("high scores", "https://localhost:44382/api/Enterprise/sessions/scores");
                return Ok(new ApiResponse<string>(response, links));
            }

            // Get session question
            Sessionquestion dbSessionQuestion = context.Sessionquestions
                .Where(q => q.IdSession == dbSession.IdSession)
                .Where(q => q.OrderIndex == dbSession.CurrentQuestionIndex)
                .FirstOrDefault();
    
            // Get question
            Question dbQuestion = context.Questions.Where(q => q.IdQuestion == dbSessionQuestion.IdQuestion).FirstOrDefault();

            // get the location
            bool inRange = false;
            if (dbSession.Latitude  >= dbQuestion.Latitude  - 10 && dbSession.Latitude  <= dbQuestion.Latitude  + 10 &&
                dbSession.Longitude >= dbQuestion.Longitude - 10 && dbSession.Longitude <= dbQuestion.Longitude + 10)
            {
                inRange = true;
            }

            // Pass
            if (answercharacter == "PASS")
            {
                // move to next question 
                dbSession.CurrentQuestionIndex++;
                context.SaveChanges();
                string response = "You have passed on this question";

                Dictionary<string, string> links = new Dictionary<string, string>();
                links.Add("question", "https://localhost:44382/api/Enterprise/question/{idsession}");
                return Ok(new ApiResponse<string>(response, links));
            }

            // Location fail
            if (dbQuestion.hasLocation == true && inRange == false)
            {
                string response = "You are in the wrong location";

                Dictionary<string, string> links = new Dictionary<string, string>();
                links.Add("question", "https://localhost:44382/api/Enterprise/session/{idsession}/location/{latitude}/{longitude}");
                return Ok(new ApiResponse<string>(response, links));
            }

            // Correct/incorrect answer
            if (dbQuestion.CorrectAnswer == answercharacter || dbQuestion.HasPicture == true)
            {
                // Get score to add
                int scoreToAdd = 2;
                if (dbSessionQuestion.TextHintUsed == true) { scoreToAdd--; }
                if (dbSessionQuestion.LocationHintUsed == true) { scoreToAdd--; }

                // Add score
                dbSession.Score += scoreToAdd;

                // Next question
                dbSession.CurrentQuestionIndex++;
                context.SaveChanges();

                // Return
                if (dbSession.CurrentQuestionIndex >= dbSession.QuestionCount)
                {
                    // End session
                    dbSession.EndSession = true;

                    // Delete session questions
                    var sessionQuestions = context.Sessionquestions;
                    var sessionQuestionsToDelete = context.Sessionquestions.Where(s => s.IdSession == dbSession.IdSession);
                    foreach(Sessionquestion sessionQuestion in sessionQuestionsToDelete)
                    {
                        sessionQuestions.Remove(sessionQuestion);
                    }
                    context.SaveChanges();

                    // Response
                    string response = "You answered correctly. Quiz now finished. Final score: " + dbSession.Score;
                    Dictionary<string, string> links = new Dictionary<string, string>();
                    links.Add("score", "https://localhost:44382/api/Enterprise/score/{idsession}");
                    links.Add("high scores", "https://localhost:44382/api/Enterprise/sessions/scores");
                    return Ok(new ApiResponse<string>(response, links));
                }
                else
                {
                    string response = "You answered correctly. Score now: " + dbSession.Score;

                    Dictionary<string, string> links = new Dictionary<string, string>();
                    links.Add("question", "https://localhost:44382/api/Enterprise/question/{idsession}");
                    links.Add("score", "https://localhost:44382/api/Enterprise/score/{idsession}"); 
                    return Ok(new ApiResponse<string>(response, links));
                }
            }
            else
            {
                string response = "You answered incorrectly. Score remains: " + dbSession.Score;
                return Ok(new ApiResponse<string>(response, new Dictionary<string, string>()));
            }
        }



        ///////////////////////////// Question Hints /////////////////////////////

        [HttpGet] // attribute
        [Route("question/textHint/{idsession}")]
        public IActionResult GetHint(int idsession)
        {
            Session dbSession = context.Sessions.Where(session => session.IdSession == idsession).Single();

            // Return immediately if session is over
            if (dbSession.EndSession == true)
            {
                string response = "Session has ended. Your score is " + dbSession.Score;

                Dictionary<string, string> DictionaryLinks = new Dictionary<string, string>();
                DictionaryLinks.Add("score", "https://localhost:44382/api/Enterprise/score/{idsession}");
                DictionaryLinks.Add("high scores", "https://localhost:44382/api/Enterprise/sessions/scores");
                return Ok(new ApiResponse<string>(response, DictionaryLinks));
            }

            Sessionquestion sessionQuestion = context.Sessionquestions
                .Where(q => q.IdSession == dbSession.IdSession)
                .Where(q => q.OrderIndex == dbSession.CurrentQuestionIndex)
                .ToList().FirstOrDefault();

            Question question = context.Questions.Where(question => question.IdQuestion == sessionQuestion.IdQuestion).Single();

            string hint = question.Hint;

            sessionQuestion.TextHintUsed = true;

            context.SaveChanges();

            Dictionary<string, string> links = new Dictionary<string, string>();
            links.Add("question", "https://localhost:44382/api/Enterprise/question/{idsession}");
            return Ok(new ApiResponse<string>(hint, links));
        }

        [HttpGet] // attribute
        [Route("session/{idsession}/sessionquestion/locationHint/")]
        public IActionResult GetLocationHint(int idsession)
        {
            // Get session
            Session dbSession = context.Sessions.Where(session => session.IdSession == idsession).Single();

            // Return immediately if session is over
            if (dbSession.EndSession == true)
            {
                string response = "Session has ended. Your score is " + dbSession.Score;
                return Ok(new ApiResponse<string>(response, new Dictionary<string, string>()));
            }

            // Get session question
            Sessionquestion sessionQuestion = context.Sessionquestions
                .Where(q => q.IdSession == dbSession.IdSession)
                .Where(q => q.OrderIndex == dbSession.CurrentQuestionIndex)
                .ToList().FirstOrDefault();

            // Get question
            Question question = context.Questions.Where(question => question.IdQuestion == sessionQuestion.IdQuestion).Single();

            // Get location
            float latitude = question.Latitude;
            float longitude = question.Longitude;

            // Set hit to used
            sessionQuestion.LocationHintUsed = true;

            // Save changes
            context.SaveChanges();

            // Return
            Dictionary<string, string> links = new Dictionary<string, string>();
            links.Add("session", "https://localhost:44382/api/Enterprise/sessions");
            links.Add("question", "https://localhost:44382/api/Enterprise/question/{idsession}");
            return Ok(new ApiResponse<Location>(new Location(latitude, longitude), links));
        }

        ///////////////////////////// Location /////////////////////////////

        [HttpPatch] // attribute
        [Route("session/{idsession}/location/{latitude}/{longitude}")]
        public IActionResult UpdateLocation(int latitude, int longitude, int idsession)
        {
            // Get tables
            Session dbSession = context.Sessions.Where(session => session.IdSession == idsession).Single();
            dbSession.Latitude = latitude;
            dbSession.Longitude = longitude;
            context.SaveChanges();
            return Ok();
        }
    }
    
    // making a model
    public record CategoryWithQuestion(Category Categories, int QuestionCount);
    public record AnswerWithQuestion(string AnswerCharacter, int? IdQuestion); 
    public record ApiResponse<T>(T body, Dictionary<string, string> links);

    public record SessionAsHighscore(string appId, string username, int score);
    public record Location(float latitude, float longitude);
}

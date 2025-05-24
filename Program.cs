using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FIleHandling
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			Console.WriteLine(">>Welcome to the File Management System<<");
			const string OPTIONS = "Choose an option:\n" +
				" 1. Search Movies using Keyword\n" +
				" 2. Search Users using Keyword\n" +
				" 3. Search Ratings using Keyword\n" +
				" 4. Exit\n" +
				">>>> ";

			int option = 0;
			bool exit = false;
			FIleHandlingDbContext context = new FIleHandlingDbContext();


			while (!exit)
			{
				try
				{
					Console.Write(OPTIONS);
					string input = Console.ReadLine();

					option = int.Parse(input);
					switch (option)
					{
						case 1:
							await FindMovieAsync();
							break;
						case 2:
							await FindUserAsync();
							break;
						case 3:
							await FindRatingAsync();
							break;
						case 4:
							exit = true;
							Console.WriteLine("\nYou have exited the app.\n");
							continue;
						default:
							Console.BackgroundColor = ConsoleColor.Red;
							Console.ForegroundColor = ConsoleColor.Black;
							Console.WriteLine("\nWrong Input, Try Again.\n");
							Console.BackgroundColor = ConsoleColor.Black;
							Console.ForegroundColor = ConsoleColor.White;
							continue;
					}
				}
				catch (FormatException)
				{
					Console.BackgroundColor = ConsoleColor.Red;
					Console.ForegroundColor = ConsoleColor.Black;
					Console.WriteLine("\nPlease enter a valid integer.\n");
					Console.BackgroundColor = ConsoleColor.Black;
					Console.ForegroundColor = ConsoleColor.White;
				}
				catch (Exception ex)
				{
					Console.BackgroundColor = ConsoleColor.Red;
					Console.ForegroundColor = ConsoleColor.Black;
					Console.WriteLine($"\nAn unexpected error occured: {ex.Message}\n");
					Console.BackgroundColor = ConsoleColor.Black;
					Console.ForegroundColor = ConsoleColor.White;
				}
			}

			await AddDataFromFilesAsync(context);
		}


		//method to add the data into the database
		private async static Task AddDataFromFilesAsync(FIleHandlingDbContext context)
		{
			string[] filePaths = {
			"./Data/Movies.txt",
			"./Data/Users.txt",
			"./Data/Ratings.txt"
			};

			foreach (var filePath in filePaths)
			{
				try
				{
					using (StreamReader reader = new StreamReader(filePath))
					{
						string line;
						while ((line = await reader.ReadLineAsync()) != null)
						{
							string[] lineParts = line.Split('|');
							string[] ratingPart = line.Split('\t');

							if (filePath.Contains("Movies.txt"))
							{
								Movie movie = new Movie
								{
									Title = lineParts[1],
									ReleaseDate = DateOnly.Parse(lineParts[2]),
									ImdbLink = lineParts[3],
									Action = int.Parse(lineParts[4]) != 0,
									Adventure = int.Parse(lineParts[5]) != 0,
									Comedy = int.Parse(lineParts[6]) != 0,
									Drama = int.Parse(lineParts[7]) != 0,
									Romance = int.Parse(lineParts[8]) != 0,
									Thriller = int.Parse(lineParts[9]) != 0,
									ScienceFiction = int.Parse(lineParts[10]) != 0,
									Animation = int.Parse(lineParts[11]) != 0,
									Fantasy = int.Parse(lineParts[12]) != 0,
									Horror = int.Parse(lineParts[13]) != 0,
									Musical = int.Parse(lineParts[14]) != 0,
									Mystery = int.Parse(lineParts[15]) != 0,
									Documentary = int.Parse(lineParts[16]) != 0,
									War = int.Parse(lineParts[17]) != 0,
									Crime = int.Parse(lineParts[18]) != 0,
									Western = int.Parse(lineParts[19]) != 0,
									FilmNoir = int.Parse(lineParts[20]) != 0,
									Childrens = int.Parse(lineParts[21]) != 0,
									Other = int.Parse(lineParts[22]) != 0
								};

								bool movieExists = await context.Movies.AnyAsync(m => m.Title == movie.Title);

								if (!movieExists)
								{
									try
									{
										await context.Movies.AddAsync(movie);
										await context.SaveChangesAsync();
										Console.WriteLine($"\nMovie has been successfully added to the database.\n");
									}
									catch (DbUpdateException dbEx)
									{
										Console.WriteLine($"\nAn error occurred while saving the movie '{movie.Title}': {dbEx.Message}\n");
									}
								}
								else
								{
									Console.WriteLine($"\nMovie already exists in the database.\n");
								}
							}
							else if (filePath.Contains("Users.txt"))
							{
								User user = new User
								{
									Age = int.Parse(lineParts[1]),
									Gender = lineParts[2],
									Occupation = lineParts[3],
									ZipCode	= lineParts[4]
								};

								bool userExists = await context.Users.AnyAsync(u => u.Age == user.Age && u.Gender == user.Gender && u.Occupation == user.Occupation && u.ZipCode == user.ZipCode);

								if (!userExists)
								{
									try
									{
										await context.Users.AddAsync(user);
										await context.SaveChangesAsync();
										Console.WriteLine($"\nUser has been successfully added to the database.\n");
									}
									catch (DbUpdateException dbEx)
									{
										Console.WriteLine($"\nAn error occurred while saving the user {dbEx.Message}\n");
									}
								}
								else
								{
									Console.WriteLine($"\nUser already exists in the database.\n");
								}
							}
							else if (filePath.Contains("Ratings.txt"))
							{
								Rating rating = new Rating
								{
									UserId = int.Parse(ratingPart[0]),
									MovieId = int.Parse(ratingPart[1]),
									Rating1 = int.Parse(ratingPart[2]),
									Timestamp = int.Parse(ratingPart[3])
								};

								bool ratingExists = await context.Ratings.AnyAsync(r => r.UserId == rating.UserId && r.MovieId == rating.MovieId);

								if (!ratingExists)
								{
									try
									{
										await context.Ratings.AddAsync(rating);
										await context.SaveChangesAsync();
										Console.WriteLine($"\nRating for User and Movie '{rating.MovieId}' has been successfully added to the database.\n");
									}
									catch (DbUpdateException dbEx)
									{
										Console.WriteLine($"\nAn error occurred while saving the rating: {dbEx.Message}\n");
									}
								}
								else
								{
									Console.WriteLine($"\nRating already exists in the database.\n");
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"\nAn unexpected error occurred while processing file '{filePath}': {ex.Message}\n");
				}
			}
		}



		private async static Task FindMovieAsync()
		{
			string path = "./Data/Movies.txt";
			Console.Write("\nType Movie name\n" +
						  ">>>> ");

			string keyword = Console.ReadLine();


			try
			{
				using (StreamReader reader = new StreamReader(path))
				{
					string line;
					List<string> matchingLines = new List<string>();

					while ((line = await reader.ReadLineAsync()) != null)
					{
						if (line.Contains(keyword.Trim()))
						{
							matchingLines.Add(line);
						}
					}

					if (matchingLines.Count == 0)
					{
						Console.WriteLine("\nNo matching movies found.\n");
					}
					else
					{
						foreach (var matchedLine in matchingLines)
						{
							string[] linePart = matchedLine.Split('|');

							if (linePart.Length >= 23) // Ensure the line has enough parts
							{

							// Display the movie details
							Console.WriteLine();
							Console.WriteLine(">>Movie Information<<\n" +
												$"- Movie Name: {linePart[1]} \n" +
												$"- Release Date: {linePart[2]} \n" +
												$"- IMDb Link: {linePart[3]}\n" +
												$"Categories: {(int.Parse(linePart[4]) != 0 ? "Action, " : "")}" +
												$"{(int.Parse(linePart[5]) != 0 ? "Adventure, " : "")}" +
												$"{(int.Parse(linePart[6]) != 0 ? "Comedy, " : "")}" +
												$"{(int.Parse(linePart[7]) != 0 ? "Drama, " : "")}" +
												$"{(int.Parse(linePart[8]) != 0 ? "Romance, " : "")}" +
												$"{(int.Parse(linePart[9]) != 0 ? "Thriller, " : "")}" +
												$"{(int.Parse(linePart[10]) != 0 ? "ScienceFiction, " : "")}" +
												$"{(int.Parse(linePart[11]) != 0 ? "Animation, " : "")}" +
												$"{(int.Parse(linePart[12]) != 0 ? "Fantasy, " : "")}" +
												$"{(int.Parse(linePart[13]) != 0 ? "Horror, " : "")}" +
												$"{(int.Parse(linePart[14]) != 0 ? "Musical, " : "")}" +
												$"{(int.Parse(linePart[15]) != 0 ? "Mystery, " : "")}" +
												$"{(int.Parse(linePart[16]) != 0 ? "Documentary, " : "")}" +
												$"{(int.Parse(linePart[17]) != 0 ? "War, " : "")}" +
												$"{(int.Parse(linePart[18]) != 0 ? "Crime, " : "")}" +
												$"{(int.Parse(linePart[19]) != 0 ? "Western, " : "")}" +
												$"{(int.Parse(linePart[20]) != 0 ? "FilmNoir, " : "")}" +
												$"{(int.Parse(linePart[21]) != 0 ? "Childrens, " : "")}" +
												$"{(int.Parse(linePart[22]) != 0 ? "Other" : "")}");
							Console.WriteLine();
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"\nAn unexpected error occurred: {ex.Message}\n");
			}

			await Task.Delay(1000);
		}


		private async static Task FindUserAsync()
		{
			string path = "./Data/Users.txt";
			Console.Write("\nType User name\n" +
						  ">>>> ");

			string keyword = Console.ReadLine();

			try
			{
				using (StreamReader reader = new StreamReader(path))
				{
					string line;
					List<string> matchingLines = new List<string>();

					while ((line = await reader.ReadLineAsync()) != null)
					{
						if (line.Contains(keyword, StringComparison.OrdinalIgnoreCase))
						{
							matchingLines.Add(line);
						}
					}

					if (matchingLines.Count == 0)
					{
						Console.WriteLine("\nNo matching users found.\n");
					}
					else
					{
						foreach (var matchedLine in matchingLines)
						{
							string[] lineParts = matchedLine.Split('|');

							if (lineParts.Length >= 6) // Ensure the line has enough parts
							{
								// Display the user details
								Console.WriteLine();
								Console.WriteLine(">>User Information<<\n" +
												  $"- User ID: {lineParts[0]} \n" +
												  $"- Age: {lineParts[1]} \n" +
												  $"- Gender: {lineParts[2]}\n" +
												  $"- Occupation: {lineParts[3]}\n" +
												  $"- Zip Code: {lineParts[4]}");
								Console.WriteLine();
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"\nAn unexpected error occurred: {ex.Message}\n");
			}

			await Task.Delay(1000);
		}


		private async static Task FindRatingAsync()
		{
			string path = "./Data/Ratings.txt";
			Console.Write("\nType User ID\n" +
						  ">>>> ");

			string userIdInput = Console.ReadLine();
			Console.Write("\nType Movie ID\n" +
						  ">>>> ");

			string movieIdInput = Console.ReadLine();

			if (!int.TryParse(userIdInput, out int userId) || !int.TryParse(movieIdInput, out int movieId))
			{
				Console.WriteLine("\nInvalid input. Please enter valid integer values for User ID and Movie ID.\n");
				return;
			}

			try
			{
				using (StreamReader reader = new StreamReader(path))
				{
					string line;
					List<string> matchingLines = new List<string>();

					while ((line = await reader.ReadLineAsync()) != null)
					{
						string[] lineParts = line.Split('\t'); // Tab-delimited

						if (lineParts.Length >= 4)
						{
							if (int.TryParse(lineParts[0], out int lineUserId) && lineUserId == userId &&
								int.TryParse(lineParts[1], out int lineMovieId) && lineMovieId == movieId)
							{
								matchingLines.Add(line);
							}
						}
					}

					if (matchingLines.Count == 0)
					{
						Console.WriteLine("\nNo matching ratings found.\n");
					}
					else
					{
						foreach (var matchedLine in matchingLines)
						{
							string[] lineParts = matchedLine.Split('\t'); // Tab-delimited

							if (lineParts.Length >= 4)
							{
								// Display the rating details
								Console.WriteLine();
								Console.WriteLine(">>Rating Information<<\n" +
												  $"- User ID: {lineParts[0]} \n" +
												  $"- Movie ID: {lineParts[1]} \n" +
												  $"- Rating: {lineParts[2]} \n" +
												  $"- Timestamp: {lineParts[3]}");
								Console.WriteLine();
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"\nAn unexpected error occurred: {ex.Message}\n");
			}

			await Task.Delay(1000);
		}

	}
}

function showRating(classes) {
    var stars_elements = document.getElementsByClassName(classes);
  
    for (let a = 0; a < stars_elements.length; a++) {
        let rating_number = stars_elements[a].querySelector("div").textContent;
        rating_number = rating_number.replace(" / 5", "");

        let stars = stars_elements[a].getElementsByTagName("i");
        // Algorithm
        let counter = 0;
        rating_number = rating_number;

        while (rating_number >= 1) {
            stars[counter].className = "fas fa-star fa-2x";
            counter++;
            rating_number--;
        }

        if (rating_number >= 0.5) {
            stars[counter].className = "fas fa-star-half-full fa-2x";
        }
    }
}

showRating("stars_ratings");

function sendRating(bookId, value) {
    var token = $("#starRatingsForm input[name='__RequestVerificationToken']").val();
    var json = { bookId: bookId, value: value };

    $.ajax({
        url: "/api/ratings",
        type: "POST",
        data: JSON.stringify(json),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'X-CSRF-TOKEN': token },
        success: function (data) {
            console.log(data);
            if (data.authenticateErrorMessage != null) {
                let authenticate_error = document.getElementById("error");
                authenticate_error.style.display = "block";
                authenticate_error.innerHTML = data.authenticateErrorMessage;
            }

            if (data.errorMessage != null) {
                let button = document.createElement("button");
                button.setAttribute("type", "button");
                button.setAttribute("data-dismiss", "alert")
                button.className = "close";
                button.innerHTML = "&times;";

                let date = convertUTCDateToLocalDate(new Date(data.nextVoteDate));
                let rating_error = document.getElementById("error");
                rating_error.style.display = "block";

                rating_error.innerHTML = data.errorMessage + " " + date.toLocaleString();
                rating_error.appendChild(button);
            }

            // Update Ratings on all divs
            let elements = document.getElementsByClassName("bookId_"+bookId);
            var starRating = document.getElementById("starRatingsSum_" + bookId);
            starRating.innerHTML = data.starRatingsSum.toFixed(1) + " / 5";
            
            showRating("stars_ratings");
        }
    });
}

function convertUTCDateToLocalDate(date) {
    var newDate = new Date(date.getTime() + date.getTimezoneOffset() * 60 * 1000);

    var offset = date.getTimezoneOffset() / 60;
    var hours = date.getHours();

    newDate.setHours(hours - offset);

    return newDate;
}
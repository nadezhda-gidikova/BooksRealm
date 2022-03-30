function showRating(classes,averageRating) {
    var stars_elements = document.getElementsByClassName(classes);

    for (let a = 0; a < stars_elements.length; a++) {
        let rating_number = stars_elements[a].querySelector("a").textContent;

        rating_number = rating_number.replace("(", "");
        rating_number = rating_number.replace(")", "");

        let stars = stars_elements[a].getElementsByTagName("i");

        // Algorithm
        let counter = 0;
        rating_number = rating_number ;
       // var averageRAting = document.getElementById("averageVoteValue").textContent;
        while (averageRating >= 1) {
            stars[counter].className = "fa fa-star";
            counter++;
            averageRating--;
        }

        if (averageRating >= 0.5) {
            stars[counter].className = "fa fa-star-half-full";
        }
    }
}

showRating("star - fill");

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
            
            // $('#averageVoteValue').html(data.averageVote.toFixed(1));
            // Update Ratings on all divs
            let elements = document.getElementsByClassName("star - fill");
            for (let a = 0; a < elements.length; a++) {
                let votes = elements[a].querySelector(".starRatingsSum_"+bookId);
                votes.innerHTML = "(" + data.starRatingsSum + ")";
            }

            showRating("book_" + bookId);
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
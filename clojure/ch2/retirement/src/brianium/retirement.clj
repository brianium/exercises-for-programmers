(ns brianium.retirement
  (:require [clojure.spec.alpha :as s]
            [clojure.string :as string]
            [brianium.retirement.core :as core]
            [brianium.retirement.input :as input])
  (:import (java.util Date))
  (:gen-class))

(defn prompt
  [text]
  (print (str text " "))
  (flush)
  (-> (read-line)
      (string/trim)))

(s/fdef prompt
  :args (s/cat :text string?)
  :ret  string?)

(defn- correction
  "Present the user with feedback about what is wrong and ask the question again"
  [state question]
  (println (input/feedback state))
  (prompt question))

(defn ask
  ([question allow-negatives?]
   (let [allowed-states (if allow-negatives? #{:success :negative} #{:success})]
     (loop [{:keys [state value]} (-> question prompt input/parse)]
       (if (allowed-states state)
         value
         (recur (-> state
                    (correction question)
                    (input/parse)))))))
  ([question]
   (ask question false)))

(s/fdef ask
  :args (s/cat :question string?)
  :ret  int?)

(defn display
  [date user]
  (let [years-until (core/years-until-retired user)]
    (println (format "You have %d years left until you can retire." years-until))
    (println (format "It's %d, so you can retire %s"
                     (core/get-year date)
                     (if (< years-until 0)
                       "now."
                       (format "in %d." (core/retire-by-year user date)))))))

(defn -main []
  (->> [(ask "What is your current age?") (ask "At what age would you like to retire?" true)]
       (apply core/make-user)
       (display (Date.))))
